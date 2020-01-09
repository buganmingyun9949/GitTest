using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine.Models.MetaParameters
{
    internal class audio
    {
        public string audioType { get; set; }
        public int sampleBytes { get; set; }
        public int sampleRate { get; set; }
        public int channel { get; set; }
        public int quality { get; set; }
        public int complexity { get; set; }
        public string compress { get; set; }
        public string vbr { get; set; }
        public saveAudio _saveAudio { get; set; }
    }
}
