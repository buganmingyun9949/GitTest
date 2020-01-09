using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Framework.Logging;
using GalaSoft.MvvmLight;
using Personal_App.Common;
using Personal_App.ViewModel;
using NAudio.Wave;
using ST.Common;

namespace Personal_App.Controls
{
    public class PlayAudioViewVm: ViewModelBase
    {

        public PlayAudioViewVm()
        {
            PlayIcon = "Play";
        }

        public PlayAudioViewVm(PlayAudioViewUC uc) : this()
        {
            this._playAudioViewUcuc = uc;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += dtimer_Tick;
        }

        #region << 字段 属性>>

        private PlayAudioViewUC _playAudioViewUcuc;

        private string _audioUrl;
        public DispatcherTimer _dTimer { get; set; }

        private string _PlayIcon;

        public string PlayIcon
        {
            get { return _PlayIcon; }
            set
            {
                _PlayIcon = value;
                RaisePropertyChanged("PlayIcon");
            }
        }

        private int _TotalTime;

        public int TotalTime
        {
            get { return _TotalTime; }
            set
            {
                _TotalTime = value;
                RaisePropertyChanged("TotalTime");
            }
        }

        private int _PlayTime;

        public int PlayTime
        {
            get { return _PlayTime; }
            set
            {
                _PlayTime = value;
                RaisePropertyChanged("PlayTime");
            }
        }

        #endregion

        #region << Btn >>


        private RelayCommand _PlayAudioCommand;//播放 音频

        public RelayCommand PlayAudioCommand
        {
            get
            {
                return _PlayAudioCommand ?? (_PlayAudioCommand = new RelayCommand(
                           (action) =>
                           {
                               //_audioUrl =
                               //    @"E:\C#\EnglishExam\LTS\trunk\LTS-PC\bin\LTS-App\Debug\Data\USER_39\simulation_7_2018032914243444274588\2018030915295115636008.mp3";

                               if (!string.IsNullOrEmpty(_playAudioViewUcuc.AudioUrl))
                                   PlayAudio(_playAudioViewUcuc.AudioUrl);
                           }));
            }
        }


        #endregion

        #region << 自定义 >>

        void dtimer_Tick(object sender, EventArgs e)
        {
            PlayTime++;

            if (PlayTime >= TotalTime)
            {
                _dTimer.Stop();
                PlayIcon = "Play";
            }
        }


        private void PlayAudio(string audioFile)
        {
            if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
            {
                GlobalUser.WavePlayer?.Stop();
                GlobalUser.WavePlayer = null;
                _dTimer.Stop();
                PlayTime = 0;
                return;
            } 

            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();
                var reader = new MediaFoundationReader(audioFile);
                GlobalUser.WavePlayer.Init(reader);
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;

                TotalTime = Convert.ToInt32(reader.TotalTime.TotalSeconds);


                PlayTime = 0;
                _dTimer.Start();

                PlayIcon = "Stop";
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
            PlayIcon = "Play";
            _dTimer?.Stop();
        }

        public void CleanUp()
        {
            if (GlobalUser.AudioFileReader != null)
            {
                GlobalUser.AudioFileReader?.Dispose();
                GlobalUser.AudioFileReader = null;
            }
            if (GlobalUser.WavePlayer != null)
            {
                GlobalUser.WavePlayer?.Stop();
                GlobalUser.WavePlayer?.Dispose();
                GlobalUser.WavePlayer = null;
            }

            _dTimer?.Stop();
        }

        #endregion
    }
}
