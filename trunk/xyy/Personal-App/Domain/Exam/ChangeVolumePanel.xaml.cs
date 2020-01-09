using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
using Personal_App.Common;
using NAudio.CoreAudioApi;
using ST.Common;
using ST.Common.ToolsHelper;
using WaveLib.AudioMixer;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// ChangeVolumeUC.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeVolumePanel : UserControl
    {
        Mixers mMixers;
        IVolumeDetector vol;
        DeviceInfo lastInfo;

        public ChangeVolumePanel()
        {
            InitializeComponent();

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

                GlobalUser.AudioVolume = vol.GetAudioVolume();

                VoiceVolumeSlider.Value = Convert.ToInt32(GlobalUser.AudioVolume * 100);


            }
            catch (Exception ex)
            {
                Log4NetHelper.ErrorFormat(this.Name, "Loaded Mixers", "加载默认声音音量失败", ex);
                MessageBox.Show("输入输出设备异常，请确认可用后重新打开程序！详细信息可查看日志。");
            }

        }
        private void VoiceVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
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
                Log4NetHelper.ErrorFormat(this.Name, "VoiceVolumeSlider_ValueChanged  加载默认声音音量失败", ex);
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
    }
}
