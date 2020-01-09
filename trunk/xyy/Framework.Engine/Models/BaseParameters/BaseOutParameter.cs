using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine.Models.BaseParameters
{
    [Serializable]
    public abstract class BaseOutParameter
    {
        public byte eof { get; set; }

        public string tokenId { get; set; }

        public string version { get; set; }

        //public BaseResult result { get; set; }
    }
}
