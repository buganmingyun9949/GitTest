using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Framework.Logging;
using Framework.VoiceRecorder.Audio.VolumeLevelIndicators;
using ST.Common.ToolsHelper;
using Personal_App.Common;
using Personal_App.ViewModel;
using Personal_App.ViewModel.Exam;
using NAudio.CoreAudioApi;
using ST.Common;
using WaveLib.AudioMixer;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// CheckHeadsetUC.xaml 的交互逻辑
    /// </summary>
    public partial class CheckHeadsetUC : UserControl
    {
        Mixers mMixers;
        IVolumeDetector vol;
        DeviceInfo lastInfo;

        public CheckHeadsetUC()
        {
            InitializeComponent();

            //TopLogoImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.logo2);

            //img1.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.话筒略低);
            //img2.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.话筒太近);
            //img3.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.录音触碰话筒);
            //img4.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.话筒太远);
            //img5.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.距离嘴巴);
            //UserInfoVM userInfoVM = new UserInfoVM();
            //userInfoVM.ExamTitle = "试听环节";
            //UserInfoUC.DataContext = userInfoVM;

            CheckInputOutputDevice(); 
        }


        private void CheckInputOutputDevice()
        {
            BindAudio();

            try
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    mMixers = new Mixers();

                    if (mMixers.Playback.Devices.Count == 0 || mMixers.Recording.Devices.Count == 0)
                    {
                        MessageBox.Show("无效的输入输出设备!");
                        return;
                    }
                }
                else
                {
                    var enumerator = new MMDeviceEnumerator();
                    var defaultMicroDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                    var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

                    if (defaultMicroDevice?.FriendlyName == null)
                    {
                        MessageBox.Show("无效的输入设备!");
                        return;
                    }
                    if (defaultDevice?.FriendlyName == null)
                    {
                        MessageBox.Show("无效的输出设备!");
                        return;
                    }
                }


                GlobalUser.AudioVolume = vol.GetAudioVolume();
                GlobalUser.RecordingVolume = vol.GetMicroVolume();

                VoiceVolumeSlider.Value = Convert.ToInt32(GlobalUser.AudioVolume * 100.0f);

                //VoiceVolumeSlider.Value = defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0f;

                MicrophoneVolumeSlider.Value = Convert.ToInt32(GlobalUser.RecordingVolume * 100.0f);
                //Convert.ToInt32(defaultMicroDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0f);


            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Loaded MMD 加载默认声音音量失败", ex);
                MessageBox.Show("无效的输入输出设备，请确认可用后重新打开程序！详细信息可查看日志。");
            }
        }


        private void MicrophoneVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                BindAudio();

                vol.ChangeMicroVolume(new DeviceInfo()
                {
                    Volume = MicrophoneVolumeSlider.Value,
                    Muted = false,
                });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("MicrophoneVolumeSlider_ValueChanged  加载默认声音音量失败", ex);
            }
        }

        private void VoiceVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                BindAudio();

                if (GlobalUser.WavePlayer != null)
                {
                    GlobalUser.AudioVolume = (float)VoiceVolumeSlider.Value * 0.01f;
                    GlobalUser.WavePlayer.Volume = GlobalUser.AudioVolume;
                }
                //vol.ChangeAudioVolume(new DeviceInfo()
                //{
                //    Volume = VoiceVolumeSlider.Value,
                //    Muted = false,
                //});
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("VoiceVolumeSlider_ValueChanged  加载默认声音音量失败", ex);
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
            vol.RegisterCallback(this.AudioChanged);
        }

        // Callback from VolumeDetector
        public void AudioChanged(DeviceInfo info)
        {
            try
            {
                // Windows7 sometimes triggers callback when volume hasn't changed. 
                if (lastInfo != null)
                {
                    if (info.Muted == lastInfo.Muted && info.Volume == lastInfo.Volume)
                        return;
                }
                lastInfo = info;
            }
            catch (ObjectDisposedException)
            {
                // In preview we recieved a callback from an item that we cannot
                // access
                return;
            }
        }

        private void DialogHost_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            //Console.WriteLine("SAMPLE 1: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));

            //you can cancel the dialog close:
            //eventArgs.Cancel();


            if (!Equals(eventArgs.Parameter, true)) return;

            GlobalUser.DoAnswer = false;
            //GlobalUser.AudioCleanUp(); //关闭音频播放
            (this.DataContext as ExamCommonVM)?.Dispose();
            this.DataContext = null;

            ButtonAutomationPeer peer =
                new ButtonAutomationPeer(CloseBtn);

            IInvokeProvider invokeProv =
                peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

            invokeProv.Invoke();

            //CloseBtn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
        }
    }
}
