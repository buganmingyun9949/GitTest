﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace VoiceRecorder.Audio
{
    public interface IAudioRecorder : IDisposable
    {
        void SetRecordTime(int recordTime);
        void BeginMonitoring(int recordingDevice = 0);
        void BeginRecording(string path);
        void Stop();

        double MicrophoneLevel { get; set; }

        RecordingState RecordingState { get; }
        SampleAggregator SampleAggregator { get; }
        event EventHandler Stopped;
        WaveFormat RecordingFormat { get; set; }
        TimeSpan RecordedTime { get; }
    }
}
