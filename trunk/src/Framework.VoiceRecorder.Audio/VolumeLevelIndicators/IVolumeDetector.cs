using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.VoiceRecorder.Audio.VolumeLevelIndicators
{
    public delegate void AudioCallback(DeviceInfo info);
    public interface IVolumeDetector
    {
        void RegisterCallback(AudioCallback callback);

        void ChangeAudioVolume(DeviceInfo info);

        void ChangeMicroVolume(DeviceInfo info);

        float GetAudioVolume();

        float GetMicroVolume();
    }
}
