using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Engine.Models.BaseParameters;

namespace Framework.Engine.Models.MetaParameters
{
    internal class commonRequest : BaseRequest
    {
        public float scale { get; set; }
        public double precision { get; set; }
    }
}
