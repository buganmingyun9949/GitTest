using Framework.Engine.Models.BaseParameters;
using Framework.Engine.Models.Results;
using System;

namespace Framework.Engine.Models.OutParameters
{
    [Serializable]
    public class predOuterParameter : BaseOutParameter
    {
        public predresult result { get; set; }
    }
}
