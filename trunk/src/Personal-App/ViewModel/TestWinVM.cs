using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ST.Common.ToolsHelper;
using Personal_App.Domain;
using Personal_App.Model;
using MaterialDesignThemes.Wpf;
using ST.Common;
using VoiceRecorder.Audio;

namespace Personal_App.ViewModel
{
    public class TestWinVM : MainViewModel
    { 

        private string _PlayUserAudioUrl;

        /// <summary>
        ///  
        /// </summary>
        public string PlayUserAudioUrl
        {
            get
            {
                return _PlayUserAudioUrl;
            }
            set
            {
                _PlayUserAudioUrl = value;
                RaisePropertyChanged("PlayUserAudioUrl");
            }
        }


        private int _PlayInt;

        /// <summary>
        ///  
        /// </summary>
        public int PlayInt
        {
            get
            {
                return _PlayInt;
            }
            set
            {
                _PlayInt = value;
                RaisePropertyChanged("PlayInt");
            }
        }

        private bool _PlayEnable;

        /// <summary>
        ///  
        /// </summary>
        public bool PlayEnable
        {
            get
            {
                return _PlayEnable;
            }
            set
            {
                _PlayEnable = value;
                RaisePropertyChanged("PlayEnable");
            }
        }


        public TestWinVM()
        {
            PlayUserAudioUrl = @"E:\C#\EnglishExam\LTS\trunk\LTS-PC\bin\LTS-App\Debug\Data\USER_39\simulation_7_2018032914243444274588\2018030915295115636008.mp3";
            PlayInt = 10;
            PlayEnable = false;



            //Sample 4
            OpenSample4DialogCommand = new RelayCommand(OpenSample4Dialog);
            AcceptSample4DialogCommand = new RelayCommand(AcceptSample4Dialog);
            CancelSample4DialogCommand = new RelayCommand(CancelSample4Dialog);



            TransTimeCommand = new RelayCommand(RunTransTimeCommand);

            //Recorder

            this.recorder = new AudioRecorder();
            this.recorder.Stopped += OnRecorderStopped;
            beginRecordingCommand = new RelayCommand(BeginRecording,
                () => recorder.RecordingState == RecordingState.Stopped ||
                      recorder.RecordingState == RecordingState.Monitoring);
            stopCommand = new RelayCommand(Stop,
                () => recorder.RecordingState == RecordingState.Recording);
            recorder.SampleAggregator.MaximumCalculated += OnRecorderMaximumCalculated;
            //Messenger.Default.Register<ShuttingDownMessage>(this, OnShuttingDown);


            if (recorder.RecordingState == RecordingState.Stopped)
            {
                BeginMonitoring(0);
            }
        }

        public new ICommand RunExtendedDialogCommand => new RelayCommand(ExecuteRunExtendedDialog);


        private void ExecuteRunExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SampleDialog2
            {
                DataContext = new SampleDialogViewModel()
            };

            //show the dialog
            DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());

            Task.Factory.StartNew(() => Thread.Sleep(3000))
                .ContinueWith(t => eventArgs.Session.Close(false), TaskScheduler.FromCurrentSynchronizationContext());
        }


        #region SAMPLE 4

        //pretty much ignore all the stuff provided, and manage everything via custom commands and a binding for .IsOpen
        public ICommand OpenSample4DialogCommand { get; }
        public ICommand AcceptSample4DialogCommand { get; }
        public ICommand CancelSample4DialogCommand { get; }

        private bool _isSample4DialogOpen;
        private object _sample4Content;

        public bool IsSample4DialogOpen
        {
            get { return _isSample4DialogOpen; }
            set
            {
                if (_isSample4DialogOpen == value) return;
                _isSample4DialogOpen = value;
                RaisePropertyChanged("IsSample4DialogOpen");
            }
        }

        public object Sample4Content
        {
            get { return _sample4Content; }
            set
            {
                if (_sample4Content == value) return;
                _sample4Content = value;
                RaisePropertyChanged("Sample4Content");
            }
        }

        private void OpenSample4Dialog(object obj)
        {
            Sample4Content = new Sample4Dialog();
            IsSample4DialogOpen = true;
        }

        private void CancelSample4Dialog(object obj)
        {
            IsSample4DialogOpen = false;
        }

        private void AcceptSample4Dialog(object obj)
        {
            //pretend to do something for 3 seconds, then close
            Sample4Content = new SampleProgressDialog();
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith((t, _) => IsSample4DialogOpen = false, null,
            //        TaskScheduler.FromCurrentSynchronizationContext());

            Task.Factory.StartNew(() => Thread.Sleep(3000))
                .ContinueWith(t => IsSample4DialogOpen = false, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        #region <<  测试 录音>>

        private readonly RelayCommand beginRecordingCommand;
        private readonly RelayCommand stopCommand;
        private readonly IAudioRecorder recorder;
        private float lastPeak;
        private string waveFileName;
        public const string ViewName = "RecorderView";

        //public RecorderViewModel(IAudioRecorder recorder)
        //{
        //    this.recorder = recorder;
        //    this.recorder.Stopped += OnRecorderStopped;
        //    beginRecordingCommand = new RelayCommand(BeginRecording,
        //        () => recorder.RecordingState == RecordingState.Stopped ||
        //              recorder.RecordingState == RecordingState.Monitoring);
        //    stopCommand = new RelayCommand(Stop,
        //        () => recorder.RecordingState == RecordingState.Recording);
        //    recorder.SampleAggregator.MaximumCalculated += OnRecorderMaximumCalculated;
        //    Messenger.Default.Register<ShuttingDownMessage>(this, OnShuttingDown);
        //}

        void OnRecorderStopped(object sender, EventArgs e)
        {
            //Messenger.Default.Send(new NavigateMessage(SaveViewModel.ViewName,
            //    new VoiceRecorderState(waveFileName, null)));
        }

        void OnRecorderMaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            lastPeak = Math.Max(e.MaxSample, Math.Abs(e.MinSample));
            RaisePropertyChanged("CurrentInputLevel");
            RaisePropertyChanged("RecordedTime");
        }

        public ICommand BeginRecordingCommand { get { return beginRecordingCommand; } }
        public ICommand StopCommand { get { return stopCommand; } }

        public void Activated(object state)
        {
            BeginMonitoring((int)state);
        }

        private void OnShuttingDown(ShuttingDownMessage message)
        {
            if (message.CurrentViewName == ViewName)
            {
                recorder.Stop();
            }
        }

        public string RecordedTime
        {
            get
            {
                var current = recorder.RecordedTime;
                return String.Format("{0:D2}:{1:D2}.{2:D3}",
                    current.Minutes, current.Seconds, current.Milliseconds);
            }
        }

        private void BeginMonitoring(int recordingDevice)
        {
            recorder.BeginMonitoring(recordingDevice);
            RaisePropertyChanged("MicrophoneLevel");
        }

        private void BeginRecording()
        {
            if (recorder.RecordingState == RecordingState.Stopped)
            {
                BeginMonitoring(0);
            }

            waveFileName = Path.Combine(GlobalUser.AUDIODATAFOLDER, Guid.NewGuid() + ".wav");
            recorder.BeginRecording(waveFileName);
            RaisePropertyChanged("MicrophoneLevel");
            //RaisePropertyChanged("ShowWaveForm");
        }

        private void Stop()
        {
            recorder.Stop();
        }

        public double MicrophoneLevel
        {
            get { return recorder.MicrophoneLevel; }
            set { recorder.MicrophoneLevel = value; }
        }

        public bool ShowWaveForm
        {
            get
            {
                return recorder.RecordingState == RecordingState.Recording ||
              recorder.RecordingState == RecordingState.RequestedStop;
            }
        }

        // multiply by 100 because the Progress bar's default maximum value is 100
        public float CurrentInputLevel { get { return lastPeak * 100; } }

        public SampleAggregator SampleAggregator
        {
            get
            {
                return recorder.SampleAggregator;
            }
        }


        #endregion


        public ICommand TransTimeCommand { get; }

        private void RunTransTimeCommand()
        {
            MessageBox.Show(TimeHelper.ConvertStringToDateTime("1519893203000").ToString("yyyy-MM-dd"));
        }
    }
}
