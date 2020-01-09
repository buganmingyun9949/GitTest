using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine.Models.Results
{
    [Serializable]
    public class recresult : BaseResult
    {
        //public string version { get; set; }

        public string textmode { get; set; }

        public string useref { get; set; }

        public string forceout { get; set; }

        public string usehookw { get; set; }

        public string conf { get; set; }

        public string starttime { get; set; }

        public string endtime { get; set; }

        public string rec { get; set; }
    }
}
