using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Engine.Models.MetaParameters;

namespace Framework.Engine.Models
{
    internal class InnerParameter
    {
        public int soundIntensityEnable { get; set; }

        public string coreProvideType { get; set; }

        public app app { get; set; }

        public audio audio { get; set; }

        public BaseRequest request { get; set; }
    }
}
