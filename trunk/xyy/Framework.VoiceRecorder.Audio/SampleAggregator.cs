﻿using System;
using System.Diagnostics;

namespace VoiceRecorder.Audio
{
    public class SampleAggregator: IDisposable
    {
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
        public event EventHandler Restart = delegate { };
        private float maxValue;
        private float minValue;
        public int NotificationCount { get; set; }
        int count;

        public void RaiseRestart()
        {
            Restart(this, EventArgs.Empty);
        }

        private void Reset()
        {
            count = 0;
            maxValue = minValue = 0;            
        }

        public void Add(float value)
        {
            maxValue = Math.Max(maxValue, value);
            minValue = Math.Min(minValue, value);
            count++;
            if (count >= NotificationCount && NotificationCount > 0)
            {
                MaximumCalculated?.Invoke(this, new MaxSampleEventArgs(minValue, maxValue));
                Reset();
            }   
        }

        public void Dispose()
        {
            Reset();
        }
    }

    public class MaxSampleEventArgs : EventArgs
    {
        [DebuggerStepThrough]
        public MaxSampleEventArgs(float minValue, float maxValue)
        {
            MaxSample = maxValue;
            MinSample = minValue;
        }
        public float MaxSample { get; private set; }
        public float MinSample { get; private set; }
    }
}
