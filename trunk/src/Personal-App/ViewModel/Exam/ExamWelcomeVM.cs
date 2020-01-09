using Framework.Logging;
using Personal_App.Common;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ST.Common;

namespace Personal_App.ViewModel.Exam
{
    public class ExamWelcomeVM : ExamCommonVM
    {
        /// <summary>
        /// 设置计时器时长。
        /// </summary>
        private const int COUNTDOWN_LENGTH = 3;
        /// <summary>
        /// 倒计时长。
        /// </summary>
        private int _countdown = 0;
        ///// <summary>
        ///// 计时器。
        ///// </summary>
        //private DispatcherTimer _dTimer;


        public const string ViewName = "ExamWelcomeView";

        /// <summary>
        /// 初始化 <see cref="ExamWelcomeVM"/> 类的新实例。
        /// </summary>
        public ExamWelcomeVM()
        {
            UserPanelVM.ExamTitle = "中考英语听说模考";

            CountdownVisibility = Visibility.Collapsed;

            CountdownTime = COUNTDOWN_LENGTH;

            PlayAudio();
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


        private bool _countdownTimeEnabled;

        public bool CountdownTimeEnabled
        {
            get
            {
                return _countdownTimeEnabled;
            }
            set
            {
                _countdownTimeEnabled = value;
                RaisePropertyChanged("CountdownTimeEnabled");
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

        private string _promptCommandText;

        /// <summary>
        /// 命令提示文本。
        /// </summary>
        public string PromptCommandText
        {
            get
            {
                return _promptCommandText;
            }
            set
            {
                _promptCommandText = value;
                RaisePropertyChanged("PromptCommandText");
            }
        }

        /// <summary>
        /// 计时器事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Countdown_Tick(object sender, EventArgs e)
        {
            _countdown--;

            CountdownTime = _countdown;

            if (_countdown < 1)
            {
                _dTimer.Stop();
                _countdown = COUNTDOWN_LENGTH;
                
                this.Dispose();
                BindExamQsUC();
            }

            //}
        }

        private void PlayAudio()
        {
            PromptCommandText = "听指令";

            GlobalUser.WavePlayer?.Stop();

            try
            {
                GlobalUser.AudioFileReader = new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Resources", "P0_1.mp3"));
                //Debug.Assert(wavePlayer == null);
                GlobalUser.WavePlayer = CreateWavePlayer();
                //audioFileReader.Volume = volumeSlider1.Volume;
                GlobalUser.WavePlayer.Init(GlobalUser.AudioFileReader);
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;
                GlobalUser.WavePlayer.Play();
                int totalTime = Convert.ToInt32(GlobalUser.AudioFileReader.TotalTime.Minutes * 60 + GlobalUser.AudioFileReader.TotalTime.Seconds);
                AudioPanelVM.SetSpeakerEnabled(totalTime);
                //TotalTime = Convert.ToInt32(GlobalUser.AudioFileReader.TotalTime.TotalSeconds);
                //_dTimer.Start();
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

        /// <summary>
        /// 开始执行倒计时。
        /// </summary>
        private void StartCountdown()
        {
            PromptCommandText = "请等待";

            // 初始化倒计时长。
            _countdown = COUNTDOWN_LENGTH;
            //CountdownVisibility = Visibility.Collapsed;
            _dTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _dTimer.Tick += Countdown_Tick;

            //CountdownTime = 0;

            _dTimer.Start();

            CountdownVisibility = Visibility.Visible;
        }

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            //_dTimer.Stop();

            GlobalUser.WavePlayer?.Stop();

            AudioPanelVM.SetAllDisable();

            // 执行倒计时
            StartCountdown();

            // we want it to be safe to clean up input stream and playback device in the handler for PlaybackStopped

            //labelNowTime.Text = "0";
            if (e.Exception != null)
            {
                Log4NetHelper.Error(String.Format("Playback Stopped due to an error {0}", e.Exception.Message));
            }
        }
    }
}
