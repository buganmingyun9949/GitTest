using ST.Common;
using Framework.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NAudio.Dsp;

namespace Framework.Recorder
{
    public class Recorder
    {
        private static readonly Recorder m_Instance = new Recorder();
        public static Recorder Instance { get { return m_Instance; } }

        #region 私有属性
        private IWaveIn m_WaveIn;
        private IWavePlayer m_WaveOut;
        private WaveFileWriter m_waveWriter;
        private AudioFileReader m_AudioReader;
        private SampleChannel m_SampleChannel;
        private RecordState m_CurrentState;

        private SampleAggregator aggregator;

        #endregion

        #region FFT

        private float maxValue;
        private float minValue;
        public bool PerformFFT { get; set; }
        private readonly Complex[] fftBuffer;
        private readonly FftEventArgs fftArgs;
        private int fftPos;
        private readonly int fftLength;
        private int m;


        public event EventHandler<FftEventArgs> FftCalculated;

        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
        

        #endregion

        #region 公有属性
        public event VolumeMeterEventHandler PreVolumeMeterHandler;
        public event VolumeMeterEventHandler PostVolumeMeterHandler;
        public event PlayFinishedHandler OnPlayFinished;

        #endregion

        #region 对外操作函数
        /// <summary>  
        /// 创建录音格式,此处使用16bit,16KHz,Mono的录音格式  
        /// <summary> 
        public void Setup()
        {
            Setup(16000, 1);
        }

        /// <summary>
        /// 创建录音格式
        /// <param name="rate">速率</param>
        /// <param name="channels">通道</param>
        /// </summary>
        public void Setup(int rate, int channels)
        {
            Cleanup();

            try
            {
                m_WaveIn = new WaveInEvent();
                if (WaveInEvent.DeviceCount == 0) throw new RecordException("没有音频设备");

                m_WaveIn.WaveFormat = new WaveFormat(rate, channels);
                NotificationCount = m_WaveIn.WaveFormat.SampleRate / 100;

                m_WaveIn.DataAvailable += OnDataAvailable;
                m_WaveIn.RecordingStopped += OnRecordingStopped;

            }
            catch (Exception e)
            {
                Cleanup();
                throw new RecordException("录音设置失败", e);
            }
        }

        /// <summary>  
        /// 开始录音  
        /// </summary> 
        public void StartRecording(string outputFileName, int rate, int channels, VolumeMeterEventHandler preVolumeMeterHandler, VolumeMeterEventHandler postVolumeMeterHandler)
        {
            Setup(rate, channels);
            PreVolumeMeterHandler = preVolumeMeterHandler;
            PostVolumeMeterHandler = postVolumeMeterHandler;
            StartRecording(outputFileName);
        }

        /// <summary>  
        /// 开始录音  
        /// </summary> 
               
        //public void StartRecording(string outputFileName, int rate, int channels, IEngine eng)
        //{
        //    ES = eng;
        //    Setup(rate, channels);
        //    StartRecording(outputFileName);
        //}
        public void StartRecording(string outputFileName, int rate, int channels)
        {
            Setup(rate, channels);
            StartRecording(outputFileName);
        }
        /// <summary>  
        /// 开始录音  
        /// </summary> 
        public void StartRecording(string outputFileName)
        {
            if (m_WaveIn == null)
            {
                Cleanup();
                throw new RecordException("请在录音前设置录音速率和通道");
            }

            try
            {
                m_waveWriter = null;
                m_waveWriter = new WaveFileWriter(outputFileName, m_WaveIn.WaveFormat);
                m_WaveIn.StartRecording();
                m_CurrentState = RecordState.RECORDING;
            }
            catch (Exception e)
            {
                Cleanup();
            }
        }

        /// <summary>  
        /// 停止录音  
        /// </summary>  
        public void StopRecording()
        {
            if (m_WaveIn != null) m_WaveIn?.StopRecording();
            Cleanup();
        }

        /// <summary>  
        /// 获取主录音设备.  
        /// </summary>  
        /// <returns>List<WaveInCapabilities)</returns>
        public List<WaveInCapabilities> RecordDevices()
        {
            List<WaveInCapabilities> devices = new List<WaveInCapabilities>();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                devices.Add(WaveIn.GetCapabilities(i));
            }

            return devices;
        }

        /// <summary>  
        /// 获取主录音设备.  
        /// </summary>  
        /// <returns>List<WaveInCapabilities)</returns>
        public List<WaveOutCapabilities> SoundDevices()
        {
            List<WaveOutCapabilities> devices = new List<WaveOutCapabilities>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                devices.Add(WaveOut.GetCapabilities(i));
            }

            return devices;
        }

        /// <summary>  
        /// 获取播放中设备音量.  
        /// </summary>  
        /// <returns>Volume</returns>
        public float Volume
        {
            get { return m_SampleChannel != null ? m_SampleChannel.Volume : 0; }
            set
            {
                if (m_SampleChannel != null)
                {
                    m_SampleChannel.Volume = value;
                }
            }
        }

        public RecordState State { get { return m_CurrentState; } }

        /// <summary>  
        /// 获取主录音设备.  
        /// </summary>  
        /// <returns>调用成功返回true,否则返回false</returns> 
        public void Dispose()
        {
            Cleanup();
        }

        #endregion

        #region 私有函数
        /// <summary>  
        /// 构造函数,设定录音设备,设定录音格式.  
        /// <summary> 
        private Recorder()
        {
            m_CurrentState = RecordState.IDLE;
        }

        private void Cleanup()
        {
            if (MaximumCalculated != null)
            {
                MaximumCalculated = null;
            }

            if (m_WaveIn != null)
            {
                if (m_CurrentState == RecordState.RECORDING)
                {
                    m_WaveIn.StopRecording();
                }

                m_WaveIn.Dispose();
                m_WaveIn = null;
            }

            if (m_SampleChannel != null)
            {
                m_SampleChannel = null;
            }

            if (m_WaveOut != null)
            {
                m_WaveOut.Dispose();
                m_WaveOut = null;
            }

            if (aggregator != null)
            {//波形
                aggregator.Reset();
                aggregator = null;
            }

            PreVolumeMeterHandler = null;
            PostVolumeMeterHandler = null;

            m_CurrentState = RecordState.STOPPED;

            if (m_waveWriter != null)
            {
                m_waveWriter.Close();
                m_waveWriter.Dispose();
            }
        }

        private void FinalizeWaveFile()
        {
            if (m_waveWriter != null)
            {
                try
                {
                    m_waveWriter.Dispose();
                }
                catch (Exception)
                {
                }
                m_waveWriter = null;
            }

            if (m_AudioReader != null)
            {
                try
                {
                    m_AudioReader.Dispose();
                }
                catch (Exception)
                {
                }
                m_AudioReader = null;
            }
        }

        int count;
        public int NotificationCount { get; set; }

        private double[] RealIn { get; set; }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {

                if ((int) e.Buffer.Length != 0)
                {
                    //if(ES!=null)
                    //{
                    //    ES.Feed(e.Buffer);
                    //}
                    m_waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
                    //RealIn = new double[e.BytesRecorded];
                    float max = 0f;

                    for (int index = 0; index < (e.BytesRecorded / 2); index += 2)
                    {
                        short sample = (short)unchecked((e.Buffer[index + 1] << 8) | (e.Buffer[index]));
                        //RealIn[index] = (double)sample;

                        //float sample16 = sample / 32768f;

                        //if (sample < 0)
                        //{
                        //    max = Math.Min(sample16, max);
                        //}
                        //else
                        //{
                        //    max = Math.Max(sample16, max);
                        //}

                        Add(sample);
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("录音文件创建失败", ex);
            }
        }
        /// <summary>
        /// RealIn是FFT需要用到的Double[]
        /// waveData是byte[]，但是是通过 Marshal.Copy得到的。。解析无力。。
        /// _numSamples是FFT需要的 UInt32 NumSamples
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="cbSize"></param>
        /// <param name="wfmt"></param>
        /// <returns></returns>
        bool GetAudioData(IntPtr ptr, int cbSize, WaveFormat wfmt)
        {
            bool samplesReady = false;
            if (cbSize == 0)
                return false; // no data
            //switch (wfmt.BitsPerSample)
            //{
            //    case 8:
            //        {
            //            // NOTE: waveData member is necessary to prevent using 'unsafe' code block
            //            Marshal.Copy(ptr, waveData, 0, (int)cbSize);
            //            if (wfmt.Channels == 1) // mono
            //            {
            //                for (uint i = 0; i < cbSize; ++i)
            //                {
            //                    RealIn[i] = (double)((waveData[i] - 128) << 6);// Out = (In-128)*64
            //                    // LeftIn[i] = RealIn[i]; // Out = (In-128)*64
            //                }
            //                _numSamples = (uint)cbSize;
            //            }
            //            else if (wfmt.Channels == 2) // stereo
            //            {
            //                // Stereo has Right+Left channels
            //                int Samples = cbSize >> 1;
            //                for (uint i = 0, j = 0; i < Samples; ++i, j += 2)
            //                {
            //                    RealIn[i] = (double)((waveData[j] - 128) << 6); // Out = (In-128)*64
            //                    // LeftIn[i] = (double)((waveData[j+1]-128)<<6); // Out = (In-128)*64
            //                }
            //                _numSamples = (uint)Samples;
            //            }
            //            samplesReady = (_numSamples != 0);
            //        }
            //        break;
            //    case 16:
            //        {
            //            // NOTE: waveData member is necessary to prevent using 'unsafe' code block
            //            Marshal.Copy(ptr, waveData, 0, (int)cbSize);
            //            if (wfmt.Channels == 1) // mono
            //            {
            //                int Samples = cbSize >> 1;
            //                for (uint i = 0, j = 0; i < Samples; ++i, j += 2)
            //                {
            //                    short val = (short)unchecked(((waveData[j + 1] << 8) + waveData[j]));
            //                    RealIn[i] = (double)val;
            //                }
            //                _numSamples = (uint)Samples;
            //            }
            //            else if (wfmt.Channels == 2) // stereo
            //            {

            //                // Stereo has Right+Left channels
            //                int Samples = cbSize >> 2;
            //                savedata.Add(new double[Samples]);
            //                for (uint i = 0, j = 0; i < Samples; ++i, j += 4)
            //                {
            //                    short val = unchecked((short)((waveData[j + 1] << 8) + waveData[j])); // right
            //                    RealIn[i] = (double)val;
            //                    savedata[savedata.Count - 1][i] = RealIn[i];


            //                    // val = unchecked((short)((waveData[j+3] << 8) + waveData[j+2])); // left
            //                }
            //                _numSamples = (uint)Samples;
            //            }
            //            samplesReady = (_numSamples != 0);
            //        }
            //        break;
            //    default:
            //        System.Diagnostics.Debug.Assert(false, "Format not supported"); // not supported
            //        break;
            //}
            return samplesReady;
        }

        private void Add(float value)
        {
            float sample16 = value / 32768f;

            if (PerformFFT && FftCalculated != null)
            {
                fftBuffer[fftPos].X = (float)(sample16 * FastFourierTransform.HammingWindow(fftPos, fftLength));
                fftBuffer[fftPos].Y = 0;
                fftPos++;
                if (fftPos >= fftBuffer.Length)
                {
                    fftPos = 0;
                    // 1024 = 2^10
                    FastFourierTransform.FFT(true, m, fftBuffer);
                    FftCalculated(this, fftArgs);
                }
            }

            maxValue = Math.Max(maxValue, sample16);
            minValue = Math.Min(minValue, sample16);
            //MaximumCalculated(this, new MaxSampleEventArgs(minValue, maxValue));
            //Reset();
            count++;
            if (count >= NotificationCount && NotificationCount > 0)
            {
                MaximumCalculated?.Invoke(this, new MaxSampleEventArgs(minValue, maxValue));
                Reset();
            }
        }

        public void Reset()
        {
            count = 0;
            maxValue = minValue = 0;
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                Log4NetHelper.ErrorFormat("录音异常停止. cause by:{0}", e.Exception.ToString());
            }

            FinalizeWaveFile();
            //if(ES!=null)
            //{
            //    ES.Stop();
            //}
            Cleanup();
        }

        void OnAudioPlayStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                Log4NetHelper.ErrorFormat("播放异常停止. cause by:{0}", e.Exception.ToString());
            }

            FinalizeWaveFile();

            Cleanup();

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

        ISampleProvider CreateSampleProvider(string fileName)
        {
            m_AudioReader = new AudioFileReader(fileName);

            m_SampleChannel = new SampleChannel(m_AudioReader, true);
            m_SampleChannel.PreVolumeMeter += OnPreVolumeMeter;
            var postVolumeMeter = new MeteringSampleProvider(m_SampleChannel);
            postVolumeMeter.StreamVolume += OnPostVolumeMeter;

            return postVolumeMeter;
        }

        private void OnPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            PreVolumeMeterHandler?.Invoke(new VolumeMeterEventArgs(e.MaxSampleValues[0], 0));
        }

        private void OnPostVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            PostVolumeMeterHandler?.Invoke(new VolumeMeterEventArgs(e.MaxSampleValues[0], 0));
        }

        #endregion
    }
}
