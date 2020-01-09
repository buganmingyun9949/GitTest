using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace VoiceRecorder.Audio
{
    public interface IAudioPlayer : IDisposable
    {
        void LoadFile(string path);
        void Play();
        void Stop();
        TimeSpan CurrentPosition { get; set; }
        TimeSpan StartPosition { get; set; }
        TimeSpan EndPosition { get; set; }

        /// <summary>
        /// Indicates that playback has gone into a stopped state due to 
        /// reaching the end of the input stream or an error has been encountered during playback
        /// </summary>
        event EventHandler PlaybackStopped;
    }
}
