using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using ST.Common;
using Framework.Logging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Framework.Recorder;

namespace LTS.Framework.Recorder
{
    public class AudioPlayback : IDisposable
    {

        private static readonly AudioPlayback m_Instance = new AudioPlayback();
        public static AudioPlayback Instance { get {
                return m_Instance;

            } }

        private IWavePlayer playbackDevice;
        private WaveStream fileStream;
        public PlaybackState PlayStatus
        {
            get
            {
                return (playbackDevice == null ? PlaybackState.Stopped : playbackDevice.PlaybackState);
            }
        }
        Image img;
        private void PlaybackDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if(img!=null)
            {
                img.Source = new BitmapImage(new Uri("/Resources/play.png", UriKind.Relative));
            }
        }

        //0 stop 1 play 2pause
        public int AudioState { get { return Convert.ToInt32(playbackDevice.PlaybackState); } }

        public double TotalTime { get { return fileStream != null ? fileStream.TotalTime.TotalSeconds : 0; } }

        public event EventHandler<FftEventArgs> FftCalculated;

        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        public event PlayFinishedHandler OnPlayFinished;
        public bool IsDeviceNull { get { return playbackDevice == null ? true : false; } }
        
        protected virtual void OnFftCalculated(FftEventArgs e)
        {
            EventHandler<FftEventArgs> handler = FftCalculated;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnMaximumCalculated(MaxSampleEventArgs e)
        {
            EventHandler<MaxSampleEventArgs> handler = MaximumCalculated;
            if (handler != null) handler(this, e);
        }

        #region Audio Play

        public void Load(string fileName)
        {

            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile(fileName);
        }
        public void LoadNoDes(string fileName)
        {

            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFileNoDes(fileName);
        }
        public void LoadNoDes(string fileName,Image image)
        {
            if (img != null)
            {
                img.Source = new BitmapImage(new Uri("/Resources/play.png", UriKind.Relative));
            }
            img = image;
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFileNoDes(fileName);
        }
        private void CloseFile()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
        }

        private void OpenFile(string fileName)
        {
            try
            {
                string outFile = Path.GetTempFileName();
                DESHelper.DESFileClass.DecryptFile(fileName, outFile, "www.17kouyu.com");
                var inputStream = new AudioFileReader(outFile);
                //var inputStream = new AudioFileReader(fileName);
                fileStream = inputStream;
                var aggregator = new SampleAggregator(inputStream);
                aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                aggregator.PerformFFT = true;
                aggregator.FftCalculated += (s, a) => OnFftCalculated(a);
                aggregator.MaximumCalculated += (s, a) => OnMaximumCalculated(a);
                playbackDevice.Init(aggregator);
            }
            catch (Exception e)
            {
                Log4NetHelper.ErrorFormat("OpenFile文件打开失败. cause by:{0}", e.Message);
                //MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }

        private void OpenFileNoDes(string fileName)
        {
            try
            {
                var inputStream = new AudioFileReader(fileName);
                fileStream = inputStream;
                var aggregator = new SampleAggregator(inputStream);
                aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                aggregator.PerformFFT = true;
                aggregator.FftCalculated += (s, a) => OnFftCalculated(a);
                aggregator.MaximumCalculated += (s, a) => OnMaximumCalculated(a);
                playbackDevice.Init(aggregator);
            }
            catch (Exception e)
            {
                Log4NetHelper.ErrorFormat("OpenFileNoDes文件打开失败. cause by:{0}", e.Message);
                //MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }
        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
            {
                CreateDevice();
            }
            else
            {
                playbackDevice.Dispose();
                CreateDevice();
            }
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200 };
        }
        List<object> btns = new List<object>();
        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                if(img!=null)
                {
                    img.Source = new BitmapImage(new Uri("/Resources/stop.png", UriKind.Relative));
                }

                playbackDevice.PlaybackStopped += PlaybackDevice_PlaybackStopped;
                playbackDevice.Play();

                //playbackDevice.PlaybackStopped += OnAudioPlayStopped;
            }
        }
        public void Play(object obj)
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                playbackDevice.Play();
                btns.Add(obj);
                //playbackDevice.PlaybackStopped += OnAudioPlayStopped;
            }
        }
        public void Play(object obj,Image image)
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                img = image;
                playbackDevice.Play();
                btns.Add(obj);
                //playbackDevice.PlaybackStopped += OnAudioPlayStopped;
            }
        }
        public List<object> GetBtn()
        {
            return btns;
        }
        public void Play(PlayFinishedHandler onPlayFinished = null, VolumeMeterEventHandler preVolumeMeterHandler = null, VolumeMeterEventHandler postVolumeMeterHandler = null)
        {

            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                playbackDevice.Play();

                if (onPlayFinished != null)
                {
                    OnPlayFinished = onPlayFinished;
                    playbackDevice.PlaybackStopped += OnAudioPlayStopped;
                }
            }
        }

        public void Pause()
        {
            if (playbackDevice != null)
            {
                playbackDevice.Pause();
            }
        }

        public void Stop()
        {
            if (MaximumCalculated != null)
            {
                MaximumCalculated = null;
            }

            if (FftCalculated != null)
            {
                FftCalculated = null;
            }

            if (playbackDevice != null)
            {
                playbackDevice.Stop();
                try
                {
                    playbackDevice.PlaybackStopped -= OnAudioPlayStopped;
                }
                catch (Exception) { }
            }
            if (fileStream != null)
            {
                try
                {
                    fileStream.Position = 0;
                }
                catch (Exception)
                {
                    fileStream = null;
                }
            }
        }
        
        void OnAudioPlayStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                Log4NetHelper.InfoFormat("播放异常停止. cause by:{0}", e.Exception.ToString());
            }

            Stop();
            CloseFile();

            if (OnPlayFinished != null)
            {
                OnPlayFinished.Invoke();
            }
            else
            {
                Log4NetHelper.Info("no play finished handler...");
            }

            //Cleanup();
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            if (playbackDevice != null)
            {
                //playbackDevice.Dispose();
                playbackDevice = null;
            }
        }

        #endregion
    }
}
