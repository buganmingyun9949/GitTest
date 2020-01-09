using System;
using System.Collections.Generic;
using System.Text;
using Framework.Logging;
using WaveLib.AudioMixer;

namespace Framework.VoiceRecorder.Audio.VolumeLevelIndicators
{
    public class VolumeDetectorXP : IVolumeDetector
    {
        private AudioCallback _callback;
        private Mixers Mixers;


        public VolumeDetectorXP()
        {
            Mixers = new Mixers(true);
        }
        #region IVolumeDetector Members

        public float GetAudioVolume()
        {
            if (Mixers != null)
            {
                return Mixers.Playback.UserLines[0].Volume  / 65535;
            }

            return 0.35f;
        }

        public float GetMicroVolume()
        {
            if (Mixers != null)
            {
                //Log4NetHelper.Info(Mixers.Recording.UserLines);
                foreach (MixerLine line in Mixers.Recording.UserLines)
                {
                    if (line.ComponentType == MIXERLINE_COMPONENTTYPE.SRC_MICROPHONE)
                    {
                        return line.Volume / 65535;
                    }
                }
            }

            return 0.80f;
        }

        public void RegisterCallback(AudioCallback callback)
        {
            _callback = callback;
            Mixers.Playback.MixerLineChanged += RunCallback;
            Mixers.Recording.MixerLineChanged += RunMicroCallback;
        }

        public void ChangeAudioVolume(DeviceInfo info)
        {
            if (Mixers != null)
            {
                //Log4NetHelper.Info(Mixers.Playback.UserLines);
                Mixers.Playback.UserLines[0].Volume = Convert.ToInt32((info.Volume * 65535) / 100);
                Mixers.Playback.UserLines[0].Mute = info.Muted;
            }
        }

        public void ChangeMicroVolume(DeviceInfo info)
        {
            if (Mixers != null)
            {
                //Log4NetHelper.Info(Mixers.Recording.UserLines);
                foreach (MixerLine line in Mixers.Recording.UserLines)
                {
                    if (line.ComponentType == MIXERLINE_COMPONENTTYPE.SRC_MICROPHONE)
                    {
                        line.Volume = Convert.ToInt32((info.Volume * 65535) / 100);
                        line.Mute = info.Muted;
                        break;
                    }
                }
                //Mixers.Recording.UserLines[0].Volume = Convert.ToInt32((info.Volume * 65535) / 100);
                //Mixers.Recording.UserLines[0].Mute = info.Muted;
            }
        }

        #endregion


        private void RunCallback(Mixer mixer, MixerLine line)
        {
            int VolumeMin = Mixers.Playback.UserLines[0].VolumeMin;
            int VolumeMax = Mixers.Playback.UserLines[0].VolumeMax;
            int VolumeCurrent = Mixers.Playback.Lines[0].Volume;

            // Volume Min and Max are probably 0, and 100, but since I can't say that for sure adjust the values to be zero based
            VolumeMax = VolumeMax - VolumeMin;
            VolumeCurrent = VolumeCurrent - VolumeMin;
            VolumeMin = 0;

            double volume = ((double)VolumeCurrent) / ((double)VolumeMax);
            bool muted = Mixers.Playback.UserLines[0].Mute;

            if (_callback != null)
                _callback.Invoke(new DeviceInfo(volume, muted));
        }


        private void RunMicroCallback(Mixer mixer, MixerLine line)
        {
            MixerLine defaultLine = null;
            foreach (MixerLine itemLine in Mixers.Recording.UserLines)
            {
                if (itemLine.ComponentType == MIXERLINE_COMPONENTTYPE.SRC_MICROPHONE)
                {
                    defaultLine = itemLine;
                    break;
                }
            }
            if (defaultLine != null)
            {
                int VolumeMin = defaultLine.VolumeMin;
                int VolumeMax = defaultLine.VolumeMax;
                int VolumeCurrent = defaultLine.Volume;

                // Volume Min and Max are probably 0, and 100, but since I can't say that for sure adjust the values to be zero based
                VolumeMax = VolumeMax - VolumeMin;
                VolumeCurrent = VolumeCurrent - VolumeMin;
                VolumeMin = 0;

                double volume = ((double) VolumeCurrent) / ((double) VolumeMax);
                bool muted = defaultLine.Mute;

                if (_callback != null)
                    _callback.Invoke(new DeviceInfo(volume, muted));
            }
        }

    }
}
