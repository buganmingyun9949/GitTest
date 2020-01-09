using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Engine.Models.MetaParameters;

namespace Framework.Engine
{
    internal class BaseRequest
    {
        public object coreType { get; set; }
        public object refText { get; set; }
        public object attachAudioUrl { get; set; }
        public object dict_type { get; set; }
        public object phoneme_output { get; set; }
        public object keywords { get; set; }
        public object qClass { get; set; }
        public object qType { get; set; }
        public object getParam { get; set; }
        public object paragraph_need_word_score { get; set; }
        //public client_params client_params { get; set; }
        //public result result { get; set; }
    }
}
