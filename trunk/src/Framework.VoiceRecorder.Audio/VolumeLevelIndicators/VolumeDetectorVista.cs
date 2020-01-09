using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Framework.Logging;
using NAudio.CoreAudioApi;

namespace Framework.VoiceRecorder.Audio.VolumeLevelIndicators
{
    // Wrap the audio device information, and callback with a standard
    // object in all situations
    public class VolumeDetectorVista : IVolumeDetector
    {
        MMDeviceEnumerator devEnum;
        MMDevice defaultDevice;
        MMDevice defaultMicroDevice;

        public VolumeDetectorVista()
        {
            devEnum = new MMDeviceEnumerator();
            try
            {
                defaultDevice =
                  devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

                defaultDevice.AudioEndpointVolume.OnVolumeNotification += DelegateNotification;


                defaultMicroDevice =
                    devEnum.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);

                defaultMicroDevice.AudioEndpointVolume.OnVolumeNotification += DelegateNotification;
            }
            catch(Exception ex)
            {
                Log4NetHelper.Error("获取录音[播放]设备失败", ex);
            }
        }

        private AudioCallback _callback;

        public void RegisterCallback(AudioCallback callback)
        {
            _callback = callback;
        }
        public float GetAudioVolume()
        {
            if (defaultDevice != null)
            {
                return defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
            }

            return 0.35f;
        }

        public float GetMicroVolume()
        {
            if (defaultMicroDevice != null)
            {
                return defaultMicroDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
            }

            return 0.80f;
        }

        public void ChangeAudioVolume(DeviceInfo info)
        {
            if (defaultDevice != null)
            {
                float volume;
                //volume = MicroVolumeSlider.Value / 100.0f;
                float.TryParse((info.Volume / 100.0d).ToString(CultureInfo.InvariantCulture),
                    out volume);
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
                defaultDevice.AudioEndpointVolume.Mute = info.Muted;
            }
        }

        public void ChangeMicroVolume(DeviceInfo info)
        {
            if (defaultMicroDevice != null)
            {
                float volume;
                //volume = MicroVolumeSlider.Value / 100.0f;
                float.TryParse((info.Volume / 100.0d).ToString(CultureInfo.InvariantCulture),
                    out volume);
                defaultMicroDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
                defaultMicroDevice.AudioEndpointVolume.Mute = info.Muted;
            }
        }


        void DelegateNotification(AudioVolumeNotificationData data)
        {
            if(_callback != null)
                _callback.Invoke(new DeviceInfo(data.MasterVolume, data.Muted));
        }

    }
}
