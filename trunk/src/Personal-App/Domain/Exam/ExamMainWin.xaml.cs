using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Framework.Logging;
using Framework.VoiceRecorder.Audio.VolumeLevelIndicators;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using NAudio.CoreAudioApi;
using Personal_App.Common;
using Personal_App.ViewModel;
using ST.Models.Paper;
using Personal_App.ViewModel.Exam;
using ST.Common;
using WaveLib.AudioMixer;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// ExamMainWin.xaml 的交互逻辑
    /// </summary>
    public partial class ExamMainWin : UserControl
    {

        public ExamType ExamType { get; set; }

        //public string SimulationId { get; set; } = "-1";

        //public SimulationPaper SimulationPaper { get; set; }

        Mixers mMixers;
        IVolumeDetector vol;
        DeviceInfo lastInfo;


        public ExamMainWin()
        {
            InitializeComponent();

        }

        public ExamMainWin(SimulationPaper simulationPaper, ExamType examType) : this()
        {
            //SimulationPaper = simulationPaper;
            this.ExamType = examType;

            //ViewModelLocator.ExamMain = ViewModelLocator.ExamMain==null? new ExamMainWinVM(GridContent, ExamType): ViewModelLocator.ExamMain;//.LoadExamMainWinVM(GridContent, ExamType);

            var vm = ViewModelLocator.ExamMain;
            vm.LoadExamMainWinVM(GridContent, ExamType);

            this.DataContext = vm;// new ExamMainWinVM(GridContent, ExamType);
            
        }

        private void ExamMainWin_OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckInputOutputDevice();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(2000);
            //timer.Tick += delegate
            //{
            //    Application.Current.Dispatcher.InvokeAsync(() => { CheckInputOutputDevice(); });
            //    timer.Stop();
            //};
            //timer.Start();
        }


        private void CheckInputOutputDevice()
        {
            BindAudio();
            string error = "";

            try
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    mMixers = new Mixers();

                    if (mMixers.Playback.Devices.Count == 0 || mMixers.Recording.Devices.Count == 0)
                    {
                        //MessageBox.Show("无效的输入输出设备!");
                        error = "请检查耳机和麦克风插头是否插好，确认可用后请退出重新开进入答题页面！";
                        //return;
                    }
                }
                else
                {
                    var enumerator = new MMDeviceEnumerator();
                    var defaultMicroDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                    var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

                    if (defaultMicroDevice?.FriendlyName == null)
                    {
                        //MessageBox.Show("无效的输入设备!");
                        error = "请检查麦克风插头是否插好，确认可用后请退出重新开进入答题页面！";
                        //return;
                    }

                    if (defaultDevice?.FriendlyName == null)
                    {
                        //MessageBox.Show("无效的输出设备!");
                        error = "请检查耳机插头是否插好，确认可用后请退出重新开进入答题页面！";
                        //return;
                    }
                }


                GlobalUser.AudioVolume = vol.GetAudioVolume();
                GlobalUser.RecordingVolume = vol.GetMicroVolume();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Loaded MMD 加载默认声音音量失败", ex);
                //MessageBox.Show("无效的输入输出设备，请确认可用后重新打开程序！详细信息可查看日志。");
                error = "请检查耳机和麦克风插头是否插好，确认可用后请退出重新开进入答题页面！";
            }
            finally
            {
                if (!string.IsNullOrEmpty(error))
                {
                    var view = new MessageDialog();
                    view.DataContext = new MessageDialogVM()
                    {
                        MsgTitle = "异常消息",
                        MsgContent = error,
                    };

                    //DispatcherTimer timer = new DispatcherTimer();
                    //timer.Interval = new TimeSpan(3000);
                    //timer.Tick += delegate
                    //{
                    //    Application.Current.Dispatcher.InvokeAsync(() => {
                    //    });
                    //    timer.Stop();
                    //};
                    //timer.Start();


                    Thread t = new Thread(() =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            Thread.Sleep(1000);//次线程休眠1秒
                            DialogHostEx.ShowDialog(this.ExamMainDialog, view, MessageDialogClose);
                        }));
                    });
                    t.Start();
                    //Messenger.Default.Send(new MainDialogMessage(this, error), "MainMessageDialog");
                }
            }
        }

        private void BindAudio()
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                // Version is 5 or less, XP or Windows Server 2k3 perhaps?
                vol = new VolumeDetectorXP();
            }
            else
            {
                // Vista on up
                vol = new VolumeDetectorVista();
            }
            //vol.RegisterCallback(this.AudioChanged);
        }

        public void MessageDialogClose(object sender, DialogOpenedEventArgs eventArgs)
        {
            Task.Factory.StartNew(() => Thread.Sleep(5000))
                .ContinueWith(t =>
                {
                    //GlobalUser.CleanUp();

                    //DialogHost.CloseAllShow(); 

                    eventArgs.Session.Close();

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
