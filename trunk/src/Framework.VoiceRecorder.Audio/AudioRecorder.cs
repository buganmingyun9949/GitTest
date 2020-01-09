using System;
using System.Linq;
using NAudio.Wave;
using NAudio.Mixer;
using Framework.Logging;

namespace VoiceRecorder.Audio
{
    public class AudioRecorder : IAudioRecorder
    {
        WaveIn waveIn;
        readonly SampleAggregator sampleAggregator;
        UnsignedMixerControl volumeControl;
        double desiredVolume = 100;
        RecordingState recordingState;
        WaveFileWriter writer;
        WaveFormat recordingFormat;
        private int recordTime = 10;

        public event EventHandler Stopped = delegate { };
        
        public AudioRecorder()
        {
            sampleAggregator = new SampleAggregator();
            RecordingFormat = new WaveFormat(16000, 1);
        }

        public WaveFormat RecordingFormat
        {
            get
            {
                return recordingFormat;
            }
            set
            {
                recordingFormat = value;
                sampleAggregator.NotificationCount = value.SampleRate / 10;
            }
        }

        /// <summary>
        /// 监控 录音 打开
        /// </summary>
        /// <param name="recordingDevice"></param>
        public void BeginMonitoring(int recordingDevice = 0)
        {
            if (recordingState != RecordingState.Stopped)
            {
                throw new InvalidOperationException("Can't begin monitoring while we are in this state: " +
                                                    recordingState.ToString());
            }

            waveIn = new WaveIn();
            //waveIn.DeviceNumber = recordingDevice;
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;
            waveIn.WaveFormat = recordingFormat;
            waveIn.StartRecording();
            //TryGetVolumeControl();
            recordingState = RecordingState.Monitoring;
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            recordingState = RecordingState.Stopped;
            writer?.Dispose();
            Stopped(this, EventArgs.Empty);
        }

        /// <summary>
        /// 录音内容 记录
        /// </summary>
        /// <param name="waveFileName"></param>
        public void BeginRecording(string waveFileName)
        {
            if (recordingState != RecordingState.Monitoring)
            {
                throw new InvalidOperationException("Can't begin recording while we are in this state: " + recordingState.ToString());
            }
            writer = new WaveFileWriter(waveFileName, recordingFormat);
            recordingState = RecordingState.Recording;
        }

        public void Stop()
        {
            if (recordingState == RecordingState.Recording)
            {
                recordingState = RecordingState.RequestedStop;
                waveIn.StopRecording();
            }
        }

        /// <summary>
        /// 配置录音时长
        /// </summary>
        /// <param name="_recordTime"></param>
        public void SetRecordTime(int _recordTime)
        {
            this.recordTime = _recordTime;
        }

        private void TryGetVolumeControl()
        {
            int waveInDeviceNumber = waveIn.DeviceNumber;
            if (Environment.OSVersion.Version.Major >= 6) // Vista and over
            {
                var mixerLine = waveIn.GetMixerLine();
                //new MixerLine((IntPtr)waveInDeviceNumber, 0, MixerFlags.WaveIn);
                foreach (var control in mixerLine.Controls)
                {
                    if (control.ControlType == MixerControlType.Volume)
                    {
                        this.volumeControl = control as UnsignedMixerControl;
                        MicrophoneLevel = desiredVolume;
                        break;
                    }
                }
            }
            else
            {
                var mixer = new Mixer(waveInDeviceNumber);
                foreach (var destination in mixer.Destinations
                    .Where(d => d.ComponentType == MixerLineComponentType.DestinationWaveIn))
                {
                    foreach (var source in destination.Sources
                        .Where(source => source.ComponentType == MixerLineComponentType.SourceMicrophone))
                    {
                        foreach (var control in source.Controls
                            .Where(control => control.ControlType == MixerControlType.Volume))
                        {
                            volumeControl = control as UnsignedMixerControl;
                            MicrophoneLevel = desiredVolume;
                            break;
                        }
                    }
                }
            }

        }

        public double MicrophoneLevel
        {
            get
            {
                return desiredVolume;
            }
            set
            {
                desiredVolume = value;
                if (volumeControl != null)
                {
                    volumeControl.Percent = value;
                }
            }
        }

        public SampleAggregator SampleAggregator
        {
            get
            {
                return sampleAggregator;
            }
        }

        public RecordingState RecordingState
        {
            get
            {
                return recordingState;
            }
        }

        public TimeSpan RecordedTime
        {
            get
            {
                if(writer == null)
                {
                    return TimeSpan.Zero;
                }
                return TimeSpan.FromSeconds((double)writer.Length / writer.WaveFormat.AverageBytesPerSecond);
            }
        }


        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            WriteToFile(buffer, bytesRecorded);

            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) |
                                        buffer[index + 0]);
                float sample32 = sample / 32768f;
                sampleAggregator.Add(sample32);
                //Log4NetHelper.Info(String.Format("SampleAggregator {0}", sample32));
            }
            //Log4NetHelper.Info(String.Format("WriteToFile {0}", bytesRecorded));
        }

        private void WriteToFile(byte[] buffer, int bytesRecorded)
        {
            long maxFileLength = this.recordingFormat.AverageBytesPerSecond * recordTime;
 
            if (recordingState == RecordingState.Recording
                || recordingState == RecordingState.RequestedStop)
            {
                var toWrite = (int)Math.Min(maxFileLength - writer.Length, bytesRecorded);
                if (toWrite > 0)
                {
                    writer.Write(buffer, 0, bytesRecorded);
                }
                else
                {
                    Stop();
                }
            }
        }
        public void Dispose()
        {
            CloseWaveIn();
            CloseInStream();
        }
        private void CloseInStream()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        private void CloseWaveIn()
        {
            if (waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }
        }
    }
}
