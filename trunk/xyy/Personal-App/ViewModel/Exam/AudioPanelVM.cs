using GalaSoft.MvvmLight;
using ST.Common.ToolsHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Personal_App.ViewModel.Exam
{
    public class AudioPanelVM : ViewModelBase
    {

        #region Private...

        /// <summary>
        /// 麦克风启用资源。
        /// </summary>
        private const string MICROPHONE_ENABLED_RESOURCES = "";
        /// <summary>
        /// 麦克风禁用资源。
        /// </summary>
        private const string MICROPHONE_DISABLE_RESOURCES = "";
        /// <summary>
        /// 扬声器启用资源。
        /// </summary>
        private const string SPEAKER_ENABLED_RESOURCES = "";
        /// <summary>
        /// 扬声器禁用资源。
        /// </summary>
        private const string SPEAKER_DISABLE_RESOURCES = "";
        /// <summary>
        /// 音量启用资源。
        /// </summary>
        private const string VOLUME_ENABLED_RESOURCES = "";
        /// <summary>
        /// 音量禁用资源。
        /// </summary>
        private const string VOLUME_DISABLE_RESOURCES = "";

        /// <summary>
        /// 图标计数。
        /// </summary>
        private int _duration = 0;
        /// <summary>
        /// 图标计时器。
        /// </summary>
        private DispatcherTimer _dispatcherTimer;

        #endregion


        public const string ViewName = "AudioDashboardView";

        /// <summary>
        /// 初始化 <see cref="AudioPanelVM"/> 类的新实例。
        /// </summary>
        public AudioPanelVM()
        {
        }


        public void SetSpeakerEnabled(int duration = 0)
        {
            MicrophoneImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.microphone_disable);
            VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_enabled);
            SpeakerImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.speaker_enabled);

            if (duration != 0)
            {
                SetVolumeAnimation(duration);
            }
        }

        public void SetVolumeEnabled()
        {
            MicrophoneImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.microphone_enabled);
            VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_enabled);
            SpeakerImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.speaker_disable);
        }

        public void SetMicrophoneEnabled()
        {
            MicrophoneImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.microphone_enabled);
            VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_disable);
            SpeakerImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.speaker_disable);
        }

        public void SetAllDisable()
        {
            _dispatcherTimer?.Stop();

            MicrophoneImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.microphone_disable);
            VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_disable);
            SpeakerImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.speaker_disable);
        }

        /// <summary>
        /// 设置音量动画。
        /// </summary>
        public void SetVolumeAnimation(int duration)
        {
            _duration = duration * 2;

            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }


        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_duration % 4 == 0)
            {
                VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_6);
            }
            else if (_duration % 3 == 0)
            {
                VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_4);
            }
            else if (_duration % 2 == 0)
            {
                VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_2);
            }
            else
            {
                VolumeImageSource = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.volume_8);
            }

            _duration--;

            if (_duration <= 0)
            {
                _dispatcherTimer?.Stop();
            }
        }

        private ImageSource _microphoneImageSource;
        /// <summary>
        /// 麦克风资源。
        /// </summary>
        public ImageSource MicrophoneImageSource
        {
            get
            {
                return _microphoneImageSource;
            }
            set
            {
                _microphoneImageSource = value;
                RaisePropertyChanged("MicrophoneImageSource");
            }
        }

        private ImageSource _speakerImageSource;
        /// <summary>
        /// 扬声器资源。
        /// </summary>
        public ImageSource SpeakerImageSource
        {
            get
            {
                return _speakerImageSource;
            }
            set
            {
                _speakerImageSource = value;
                RaisePropertyChanged("SpeakerImageSource");
            }
        }

        private ImageSource _volumeImageSource;
        /// <summary>
        /// 音量资源。
        /// </summary>
        public ImageSource VolumeImageSource
        {
            get
            {
                return _volumeImageSource;
            }
            set
            {
                _volumeImageSource = value;
                RaisePropertyChanged("VolumeImageSource");
            }
        }

        ///// <summary>
        ///// 麦克风是否启用。
        ///// </summary>
        //public bool MicrophoneEnabled { get; set; }
        ///// <summary>
        ///// 扬声器是否启用。
        ///// </summary>
        //public bool SpeakerEnabled { get; set; }
        ///// <summary>
        ///// 音量是否启用。
        ///// </summary>
        //public bool VolumeEnabled { get; set; }


    }
}
