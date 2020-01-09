using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using Personal_App.Common;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Personal_App.Model;
using MaterialDesignThemes.Wpf;
using NAudio.Wave;
using ST.Common;
using VoiceRecorder.Audio;

namespace Personal_App.ViewModel.Exam
{
    public class CheckHeadsetVM : ExamCommonVM
    {
        public CheckHeadsetVM()
        {
            UserPanelVM.ExamTitle = ExamTitle;

            ////BeginRecordingEnabled = true;

            ////放到InitDispatcherTimer()
            //PlayingTime = 1;
            //_dTimer = new DispatcherTimer();
            //_dTimer.Interval = TimeSpan.FromSeconds(1);
            //_dTimer.Tick += dtimer_Tick;
            ////放到InitDispatcherTimer()

            ShowProgressUC = Visibility.Visible;
            ShowPolygonWaveForm = Visibility.Collapsed;
            ShowWaveForm = false;

            ShowBeginRecordBtn = Visibility.Visible;
            ShowThreeBtn = Visibility.Collapsed;
            PlayRecordAgainEnabled = false;

            //PlayTipsAudio();           
            StartCountdown();//打开页面等待3秒读测试音频
        }

        private void InitDispatcherTimer()
        {
            PlayingTime = 1;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += dtimer_Tick;
        }

        #region << 字段 >>
        public const string ViewName = "CheckHeadsetView";

        private string fileName;
        private bool isTestAudio;
        private bool isMyRecord;
        private bool isTipsAudio;
        private float lastPeak;
        private string waveFileName;

        private bool isPlayMyRecord = false;

        private int recordTime = 10;//录音时长

        #endregion

        #region << 属性 >>

        private string _examTitle = "试音环节";

        /// <summary>
        /// 考试名称。 
        /// </summary>
        public string ExamTitle
        {
            get
            {
                return _examTitle;
            }
            set
            {
                _examTitle = value;
                RaisePropertyChanged("ExamTitle");
            }
        }

        private string _PromptCommandText;

        /// <summary>
        /// 命令提示 
        /// </summary>
        public string PromptCommandText
        {
            get
            {
                return _PromptCommandText;
            }
            set
            {
                _PromptCommandText = value;
                RaisePropertyChanged("PromptCommandText");
            }
        }

        private string playIcon;

        /// <summary>
        /// 播放 按钮图标 切换
        /// </summary>
        public string PlayIcon
        {
            get { return playIcon; }
            set
            {
                playIcon = value;
                RaisePropertyChanged("PlayIcon");
            }
        }

        private int _PlayingTime;

        public int PlayingTime
        {
            get
            {
                return _PlayingTime;
            }
            set
            {
                _PlayingTime = value;
                RaisePropertyChanged("PlayingTime");
            }
        }

        private int _TotalTime;

        public int TotalTime
        {
            get
            {
                return _TotalTime;
            }
            set
            {
                _TotalTime = value;
                RaisePropertyChanged("TotalTime");
            }
        }

        private Visibility _ShowProgressUC;

        /// <summary>
        /// 播放时间进度条 显示
        /// </summary>
        public Visibility ShowProgressUC
        {
            get
            {
                return _ShowProgressUC;
            }
            set
            {
                _ShowProgressUC = value;
                RaisePropertyChanged("ShowProgressUC");
            }
        }

        private Visibility _ShowBeginRecordBtn;

        /// <summary>
        /// 录音按钮 显示
        /// </summary>
        public Visibility ShowBeginRecordBtn
        {
            get
            {
                return _ShowBeginRecordBtn;
            }
            set
            {
                _ShowBeginRecordBtn = value;
                RaisePropertyChanged("ShowBeginRecordBtn");
            }
        }

        private Visibility _ShowThreeBtn;

        /// <summary>
        /// 录音后 三按钮 显示
        /// </summary>
        public Visibility ShowThreeBtn
        {
            get
            {
                return _ShowThreeBtn;
            }
            set
            {
                _ShowThreeBtn = value;
                RaisePropertyChanged("ShowThreeBtn");
            }
        }

        private Visibility _ShowPolygonWaveForm;

        /// <summary>
        /// 波形 显示
        /// </summary>
        public Visibility ShowPolygonWaveForm
        {
            get
            {
                return _ShowPolygonWaveForm;
            }
            set
            {
                _ShowPolygonWaveForm = value;
                RaisePropertyChanged("ShowPolygonWaveForm");
            }
        }


        private bool _ShowWaveForm;
        public bool ShowWaveForm
        {
            get
            {
                return _ShowWaveForm;
            }
            set
            {
                _ShowWaveForm = value;
                RaisePropertyChanged("ShowWaveForm");
            }
        }


        private bool _PlayRecordAgainEnabled;
        public bool PlayRecordAgainEnabled
        {
            get
            {
                return _PlayRecordAgainEnabled;
            }
            set
            {
                _PlayRecordAgainEnabled = value;
                RaisePropertyChanged("PlayRecordAgainEnabled");
            }
        }

        private bool _beginRecordingEnabled;
        public bool BeginRecordingEnabled
        {
            get
            {
                return _beginRecordingEnabled;
            }
            set
            {
                _beginRecordingEnabled = value;
                RaisePropertyChanged("BeginRecordingEnabled");
            }
        }


        private bool _testAudioButtonEnabled;
        public bool TestAudioButtonEnabled
        {
            get
            {
                return _testAudioButtonEnabled;
            }
            set
            {
                _testAudioButtonEnabled = value;
                RaisePropertyChanged("TestAudioButtonEnabled");
            }
        }

        private int _countdownTime;
        public int CountdownTime
        {
            get
            {
                return _countdownTime;
            }
            set
            {
                _countdownTime = value;
                RaisePropertyChanged("CountdownTime");
            }
        }

        private Visibility _countdownVisibility;
        public Visibility CountdownVisibility
        {
            get
            {
                return _countdownVisibility;
            }
            set
            {
                _countdownVisibility = value;
                RaisePropertyChanged("CountdownVisibility");
            }
        }

        private Visibility _PromptVisibility;
        public Visibility PromptVisibility
        {
            get
            {
                return _PromptVisibility;
            }
            set
            {
                _PromptVisibility = value;
                RaisePropertyChanged("PromptVisibility");
            }
        }


        #endregion

        #region << 按钮  command >>


        private RelayCommand _SkipTestCommand;//跳过测试,打开 考试

        public RelayCommand SkipTestCommand
        {
            get
            {
                return _SkipTestCommand ?? (_SkipTestCommand = new RelayCommand(
                           (action) =>
                           {
                               var view = new BeginExamDialog();
                               //show the dialog
                               var result = DialogHost.Show(view, action, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

                           }));
            }
        }

        private RelayCommand _PlayTestAudioCommand;//播放 音频

        public RelayCommand PlayTestAudioCommand
        {
            get
            {
                return _PlayTestAudioCommand ?? (_PlayTestAudioCommand = new RelayCommand(
                           (action) =>
                           {
                               //var param = action as List<object>;//BtnPlayTestAudio  PlayTestAudioIcon

                               //PlayAudio(true);
                               PromptCommandText = "听语音";
                               PlayTestAudio();

                           }));
            }
        }


        private RelayCommand _BeginRecordingCommand;//开始 录音

        public RelayCommand BeginRecordingCommand
        {
            get
            {
                return _BeginRecordingCommand ?? (_BeginRecordingCommand = new RelayCommand(
                           (action) =>
                           {
                               if (GlobalUser.Recorder?.RecordingState != RecordingState.Recording)
                               {
                                   //PlayRecorder();
                                   BeginRecordingEnabled = false;
                                   TestAudioButtonEnabled = false;
                                   BeginRecorder(FlowType.StartAudio);
                               }
                           }));
            }
        }

        private RelayCommand _GoodRecordCommand;//录音 正常,打开 考试

        public RelayCommand GoodRecordCommand
        {
            get
            {
                return _GoodRecordCommand ?? (_GoodRecordCommand = new RelayCommand(
                           (action) =>
                           {
                               var view = new BeginExamDialog();
                               //show the dialog
                               var result = DialogHost.Show(view, action, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

                           }));
            }
        }

        private RelayCommand _BadRecordCommand;//不好的录音,重新录音

        public RelayCommand BadRecordCommand
        {
            get
            {
                return _BadRecordCommand ?? (_BadRecordCommand = new RelayCommand(
                           (action) =>
                           {
                               BeginRecordingEnabled = true;
                               ShowProgressUC = Visibility.Visible;
                               ShowPolygonWaveForm = Visibility.Collapsed;
                               ShowWaveForm = false;

                               ShowBeginRecordBtn = Visibility.Visible;
                               ShowThreeBtn = Visibility.Collapsed;
                               PlayRecordAgainEnabled = false;

                               GlobalUser.WavePlayer?.Stop();
                               GlobalUser.Recorder?.Stop();
                           }));
            }
        }

        private RelayCommand _PlayRecordAgainCommand;//播放 我的录音

        public RelayCommand PlayRecordAgainCommand
        {
            get
            {
                return _PlayRecordAgainCommand ?? (_PlayRecordAgainCommand = new RelayCommand(
                           (action) =>
                           {
                               PromptCommandText = "听录音";
                               //PlayAudio(false);
                               PlayUserAudio();
                           }));
            }
        }

        #endregion

        #region << 自定义 方法 >>

        int _countdown = 0;

        /// <summary>
        /// 开始执行倒计时。
        /// </summary>
        private void StartCountdown()
        {
            CountdownVisibility = Visibility.Visible;
            PromptVisibility = Visibility.Collapsed;

            PromptCommandText = "请等待";
            _countdown = CountdownTime = 3;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += Countdown_Tick;
            _dTimer.Start();
        }

        private void Countdown_Tick(object sender, EventArgs e)
        {
            _countdown--;
            CountdownTime = _countdown;

            if (_countdown <= 0)
            {
                _dTimer.Stop();
                _countdown = 3;

                CleanUp();

                CountdownVisibility = Visibility.Collapsed;
                PromptVisibility = Visibility.Visible;

                PlayTipsAudio(); //播放测试音频
            }
        }

        private void dtimer_Tick(object sender, EventArgs e)
        {
            if (GlobalUser.AudioFileReader != null)
            {
                //PlayingTime = Convert.ToInt32(GlobalUser.AudioFileReader?.CurrentTime.Minutes * 60 +
                //                              GlobalUser.AudioFileReader?.CurrentTime.Seconds);
                PlayingTime++;
            }

            if (PlayingTime == TotalTime)
            {
                _dTimer.Stop();
            }

            if (GlobalUser.Recorder?.RecordingState == RecordingState.Recording)
            {
                recordTime--;

                if (recordTime < 1)
                {
                    _dTimer.Stop();
                    GlobalUser.Recorder.Stop();
                    recordTime = 10;
                }
            }
        }

        /// <summary>
        /// 播放提示音频。
        /// </summary>
        private void PlayTipsAudio()
        {
            //播放提示音频前初始化计时器
            InitDispatcherTimer();

            PromptCommandText = "听语音";

            GlobalUser.AudioFileReader = new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "P0.mp3"));

            isTipsAudio = true;

            TestAudioButtonEnabled = false;
            BeginRecordingEnabled = false;

            PlayAudio();
        }

        /// <summary>
        /// 播放测试音频。
        /// </summary>
        private void PlayTestAudio()
        {
            BeginRecordingEnabled = false; //设置开始录音不可用
            PlayIcon = "Stop";
            GlobalUser.AudioFileReader = new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "PTest.mp3"));
            PlayAudio(true);
        }

        /// <summary>
        /// 播放用户音频。
        /// </summary>
        private void PlayUserAudio()
        {
            //PromptCommandText = "听语音";

            isPlayMyRecord = true;

            PlayIcon = "Play";
            isTestAudio = false;

            GlobalUser.AudioFileReader = new AudioFileReader(waveFileName);
            isMyRecord = true;
            PlayRecordAgainEnabled = false;
            TestAudioButtonEnabled = false;

            PlayingTime = 1;
            PlayAudio();
        }

        /// <summary>
        /// 播放音频。
        /// </summary>
        /// <param name="_isTestAudio"></param>
        private void PlayAudio(bool _isTestAudio = false)
        {
            isTestAudio = _isTestAudio;

            ShowProgressUC = Visibility.Visible;
            ShowPolygonWaveForm = Visibility.Collapsed;

            GlobalUser.WavePlayer?.Stop();

            try
            {
                PlayingTime = 1;
                if (isPlayMyRecord)
                {
                    TotalTime = 10;
                    isPlayMyRecord = false;
                }
                else
                {
                    TotalTime = Convert.ToInt32(GlobalUser.AudioFileReader.TotalTime.TotalSeconds - 0.5);
                }
                _dTimer.Start();

                //Debug.Assert(wavePlayer == null);
                GlobalUser.WavePlayer = CreateWavePlayer();
                //audioFileReader.Volume = volumeSlider1.Volume;
                GlobalUser.WavePlayer.Init(GlobalUser.AudioFileReader);
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;
                //Thread.Sleep(Convert.ToInt32((TotalTime - GlobalUser.AudioFileReader.TotalTime.TotalSeconds) * 1000));                
                GlobalUser.WavePlayer.Play();
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(String.Format("Play Error {0}", e.Message));
            }
        }

        private IWavePlayer CreateWavePlayer()
        {
            return new WaveOut();
        }

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (isTestAudio)
            {
                PlayIcon = "Play";
                BeginRecordingEnabled = true;
            }
            if (isTipsAudio)
            {
                BeginRecordingEnabled = true;
                TestAudioButtonEnabled = true;
                isTipsAudio = false;
            }
            if (isMyRecord)
            {
                isMyRecord = false;
                PlayRecordAgainEnabled = true;
                TestAudioButtonEnabled = true; //播放完录音内容后，此时测试音频按钮可用
            }

            CleanUp();

            // we want to be always on the GUI thread and be able to change GUI components
            //Debug.Assert(!InvokeRequired, "PlaybackStopped on wrong thread");
            // we want it to be safe to clean up input stream and playback device in the handler for PlaybackStopped

            //labelNowTime.Text = "0";
            if (e.Exception != null)
            {
                Log4NetHelper.Error(String.Format("Playback Stopped due to an error {0}", e.Exception.Message));
            }
        }


        void OnPlayStartStopped(object sender, StoppedEventArgs e)
        {
            if (_nextFlowType == FlowType.StartAudio)
            {
                _dTimer.Stop();
                CleanUp();
                _nextFlowType = FlowType.DiAudio;
                BeginRecorder(_nextFlowType);
            }
            else if (_nextFlowType == FlowType.DiAudio)
            {
                _nextFlowType = FlowType.RecordTime;
                BeginRecorder(_nextFlowType);
            }
            else
            {
                _dTimer.Stop();
                CleanUp();
            }

            if (e.Exception != null)
            {
                Log4NetHelper.Error(String.Format("Playback Stopped due to an error {0}", e.Exception.Message));
            }
        }

        FlowType _nextFlowType = FlowType.StartAudio;

        private void BeginRecorder(FlowType nextFlowType)
        {
            switch (nextFlowType)
            {
                case FlowType.StartAudio:
                    //录音前播放提示音频
                    _nextFlowType = FlowType.StartAudio;
                    PromptCommandText = "听指令";

                    GlobalUser.WavePlayer?.Stop();
                    GlobalUser.AudioFileReader = new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Start.mp3"));
                    PlayingTime = 0;
                    //TotalTime = Convert.ToInt32(GlobalUser.AudioFileReader.TotalTime.TotalSeconds);
                    TotalTime = Convert.ToInt32(GlobalUser.AudioFileReader?.TotalTime.Minutes * 60 +
                                            GlobalUser.AudioFileReader?.TotalTime.Seconds);
                    _dTimer.Start();
                    GlobalUser.WavePlayer = CreateWavePlayer();
                    GlobalUser.WavePlayer.Init(GlobalUser.AudioFileReader);
                    GlobalUser.WavePlayer.PlaybackStopped += OnPlayStartStopped;
                    GlobalUser.WavePlayer.Play();
                    break;
                case FlowType.DiAudio:
                    //录音前播放提示音频
                    _nextFlowType = FlowType.DiAudio;
                    PromptCommandText = "听指令";

                    GlobalUser.WavePlayer?.Stop();
                    GlobalUser.AudioFileReader = new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Di.mp3"));
                    PlayingTime = 1;
                    TotalTime = Convert.ToInt32(GlobalUser.AudioFileReader.TotalTime.TotalSeconds);
                    _dTimer.Start();
                    GlobalUser.WavePlayer = CreateWavePlayer();
                    GlobalUser.WavePlayer.Init(GlobalUser.AudioFileReader);
                    GlobalUser.WavePlayer.PlaybackStopped += OnPlayStartStopped;
                    GlobalUser.WavePlayer.Play();
                    break;
                case FlowType.RecordTime:
                    PlayRecorder();
                    break;
                default:
                    break;
            }
        }

        private void PlayRecorder()
        {
            GlobalUser.WavePlayer?.Stop();
            PromptCommandText = "录音中";
            recordTime = 10;
            ShowProgressUC = Visibility.Collapsed;
            ShowPolygonWaveForm = Visibility.Visible;
            TestAudioButtonEnabled = false; //录音时不显示测试音频按钮

            GlobalUser.Recorder = new AudioRecorder();
            GlobalUser.Recorder.Stopped += OnRecorderStopped;
            BeginMonitoring(recordTime);
            ShowWaveForm = true;
            BeginRecording(recordTime);
            //BeginRecordingEnabled = false;
            SampleAggregator = GlobalUser.Recorder.SampleAggregator;
        }

        private void BeginMonitoring(int recordTime = 10)
        {
            GlobalUser.Recorder?.Stop();
            GlobalUser.Recorder.SetRecordTime(recordTime);
            GlobalUser.Recorder.BeginMonitoring();
            //RaisePropertyChanged("MicrophoneLevel");
        }

        private void BeginRecording(int recordTime = 10)
        {
            PlayingTime = 1;
            TotalTime = recordTime;
            _dTimer.Start();
            waveFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
            GlobalUser.Recorder.BeginRecording(waveFileName);
        }

        void OnRecorderStopped(object sender, EventArgs e)
        {
            ShowBeginRecordBtn = Visibility.Collapsed;
            ShowThreeBtn = Visibility.Visible;
            TestAudioButtonEnabled = true; //录音完成后重新显示测试音频按钮

            PlayRecordAgainEnabled = false; //录音完成自动播放录内容，此时录音回放不可用
                                            //PlayAudio(false);
                                            //Thread.Sleep(recordTime * 1000);
            PromptCommandText = "听语音";
            PlayUserAudio();
        }

        void OnRecorderMaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            lastPeak = Math.Max(e.MaxSample, Math.Abs(e.MinSample));
            RaisePropertyChanged("PlayingTime");
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {

        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            //OK, lets cancel the close...
            //eventArgs.Cancel();


            if ((bool)eventArgs.Parameter)
            {
                CleanUp();

                //Thread.SpinWait(1500);

                // 跳转到欢迎模考
                //BindExamWelcomeUC();

                BindExamQsUC();
            }
            else
            {

                //if (ShowBeginRecordBtn == Visibility.Visible)
                //    BeginRecordingEnabled = true;
                // PlayRecordAgainEnabled = true;
            }

        }

        #endregion
    }
}
