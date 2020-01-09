using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Framework.Logging;
using MaterialDesignThemes.Wpf;
using NAudio.Vorbis;
using NAudio.Wave;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;

namespace Plugin.Exam.Report.Controls
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
                //TxtPlayTime.Text = PlayTime.ToString();
            }
            else
            {
                SliderTimer.Value = PlayTime = 0;
                //TxtPlayTime.Text = "0";
                PlayIconKind.Kind = PackIconKind.Play;
                _dTimer.Stop();
            }
        }


        private void PlayAudio(string audioFile)
        {
            if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
            {
                CleanUp();

                PlayIconKind.Kind = PackIconKind.Play;
                PlayTime = 0;
                return;
            }

            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();
                if (!audioFile.Contains("http://"))
                {
                    audioFile = $"http://{audioFile}";
                }

                try
                {
                    Stream sfile;

                    string pfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER,
                        SecurityHelper
                            .HmacMd5Encrypt(GlobalUser.SelectPaperName + GlobalUser.SelectPaperNumber,
                                GlobalUser.FILEPWD, Encoding.UTF8).ToUpper(),
                        SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(audioFile), GlobalUser.FILEPWD,
                            Encoding.UTF8).ToLower() + ".qf");

                    if (!File.Exists(pfile))
                    {
                        if (Environment.OSVersion.Version.Major < 6)
                        {
                            //Vista 以下操作系统执行
                            //获取流文件 
                            byte[] audioByte = WebApiProxy.GetAudioFile($"{audioFile}.mp3");
                            sfile = new MemoryStream(audioByte);
                            Mp3FileReader mfr0 = new Mp3FileReader(sfile);

                            GlobalUser.WavePlayer = CreateWavePlayer();
                            SliderTimer.Maximum = TotalTime = Convert.ToInt32(mfr0.TotalTime.TotalSeconds);
                            TxtPlayTime.Text = TotalTime.ToString();
                            GlobalUser.WavePlayer.Init(mfr0);

                            GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;

                            SliderTimer.Value = PlayTime = 0;
                            _dTimer.Start();

                            PlayIconKind.Kind = PackIconKind.Stop;
                            GlobalUser.WavePlayer.Play();

                            return;
                        }

                        throw new Exception("无效的本地内容文件");
                    }

                    sfile = FileSecretHelper.DecryptFile0(pfile);

                    //if (!audioFile.ToLower().Contains(".mp3"))
                    //    reueUrl = $"{audioFile}.mp3";
                    Mp3FileReader mfr = new Mp3FileReader(sfile);

                    //var reader = //new MediaFoundationReader($"{audioFile}.mp3");
                    //            new AudioFileReader(Path.Combine(GlobalUser.AUDIODATAFOLDER, $"{Path.GetFileName(audioFile)}.mp3"));
                    SliderTimer.Maximum = TotalTime = Convert.ToInt32(mfr.TotalTime.TotalSeconds);
                    TxtPlayTime.Text = TotalTime.ToString();
                    GlobalUser.WavePlayer.Init(mfr);
                }
                catch (Exception ex)
                {
                    try
                    {
                        var reader = new MediaFoundationReader($"{audioFile}.mp3");
                        SliderTimer.Maximum = TotalTime = Convert.ToInt32(reader.TotalTime.TotalSeconds);
                        TxtPlayTime.Text = TotalTime.ToString();
                        GlobalUser.WavePlayer.Init(reader);
                    }
                    catch (Exception)
                    {
                        var oggReader = new VorbisWaveReader($"{audioFile}.ogg");
                        SliderTimer.Maximum = TotalTime = Convert.ToInt32(oggReader.TotalTime.TotalSeconds);
                        TxtPlayTime.Text = TotalTime.ToString();

                        GlobalUser.WavePlayer.Init(oggReader);
                    }
                }

                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;

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
                //CleanUp();
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
