using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.VoiceRecorder.Audio.VolumeLevelIndicators
{
    public class DeviceInfo
    {
        public double Volume;
        public bool Muted;
        public DeviceInfo() { }
        public DeviceInfo(double volume, bool muted)
        {
            Volume = volume;
            Muted = muted;
        }
    }
}
