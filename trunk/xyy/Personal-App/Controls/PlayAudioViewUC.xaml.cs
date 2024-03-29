﻿using System;
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
using System.Windows.Threading;
using Framework.Logging;
using Personal_App.Common;
using MaterialDesignThemes.Wpf;
using NAudio.Wave;
using ST.Common;

namespace Personal_App.Controls
{
    /// <summary>
    /// PlayAudioViewUC.xaml 的交互逻辑
    /// </summary>
    public partial class PlayAudioViewUC : UserControl
    {

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
            }
        }

        private int _TotalTime;

        public int TotalTime
        {
            get { return _TotalTime; }
            set
            {
                _TotalTime = value;
            }
        }

        private int _PlayTime;

        public int PlayTime
        {
            get { return _PlayTime; }
            set
            {
                _PlayTime = value;
            }
        }

        #endregion

        public bool IsControlEnabled
        {
            get { return (bool)GetValue(IsControlEnabledProperty); }
            set { SetValue(IsControlEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsControlEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsControlEnabledProperty =
            DependencyProperty.Register("IsControlEnabled", typeof(bool), typeof(PlayAudioViewUC),
                new PropertyMetadata(true, EnablePropertyChangedCallback));


        public string AudioUrl
        {
            get { return (string)GetValue(AudioUrlProperty); }
            set { SetValue(AudioUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AudioUrlProperty =
            DependencyProperty.Register("AudioUrl", typeof(string), typeof(PlayAudioViewUC),
                new PropertyMetadata("", AudioUrlPropertyChangedCallback));

        public PlayAudioViewUC()
        {
            InitializeComponent();
        } 

        static void AudioUrlPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PlayAudioViewUC uc = (sender as PlayAudioViewUC);
            if (uc != null)
            {
                uc.AudioUrl = e.NewValue?.ToString();
            }
        }
        static void EnablePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PlayAudioViewUC uc = (sender as PlayAudioViewUC);
            if (uc != null)
            {
                uc.IsEnabled = (bool) e.NewValue;
            }
        }

        private void PlayAudioViewUC_OnLoaded(object sender, RoutedEventArgs e)
        {
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += dtimer_Tick;

            //this.DataContext = new PlayAudioViewVm(this);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(AudioUrl))
                PlayAudio(AudioUrl);
        }

        #region << 自定义 >>

        void dtimer_Tick(object sender, EventArgs e)
        {
            if (GlobalUser.WavePlayer != null)
            {
                PlayTime++;

                if (GlobalUser.WavePlayer.PlaybackState == PlaybackState.Stopped)
                {
                    _dTimer.Stop();
                    PlayIconKind.Kind = PackIconKind.Play;
                }

                if (PlayTime >= TotalTime)
                {
                    _dTimer.Stop();
                    PlayIconKind.Kind = PackIconKind.Play;
                }

                SliderTimer.Value = PlayTime;
                TxtPlayTime.Text = PlayTime.ToString();
            }
            else
            {
                SliderTimer.Value = PlayTime = 0;
                TxtPlayTime.Text = "0";
                PlayIconKind.Kind = PackIconKind.Play;
                _dTimer.Stop();
            }
        }


        private void PlayAudio(string audioFile)
        {
            if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
            {
                GlobalUser.WavePlayer?.Stop();
                GlobalUser.WavePlayer = null;
                _dTimer.Stop();
                PlayIconKind.Kind = PackIconKind.Play;
                PlayTime = 0;
                return;
            }

            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();
                var reader = new MediaFoundationReader(audioFile);
                GlobalUser.WavePlayer.Init(reader);
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;

                SliderTimer.Maximum = TotalTime = Convert.ToInt32(reader.TotalTime.TotalSeconds);


                SliderTimer.Value = PlayTime = 0;
                _dTimer.Start();

                PlayIconKind.Kind = PackIconKind.Stop;
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
            //if (PlayTime == TotalTime)
            {
                PlayIconKind.Kind = PackIconKind.Play;
                CleanUp();
            }
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
