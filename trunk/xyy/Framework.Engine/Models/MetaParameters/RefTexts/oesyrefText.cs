﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Engine.Models.BaseParameters;
using Framework.Engine.Models.MetaParameters.answers;

namespace Framework.Engine.Models.MetaParameters
{
    internal class oesyrefText : basereftext
    {
        public oesyanswer[] lm { get; set; }
    }
}
