using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class AgentInfo
    {
        public dynamic banner { get; set; }

        public string qrcode { get; set; }

        public dynamic kefu { get; set; }
    }

    public class KeFu
    {
        public string phone { get; set; }

        public string qq { get; set; }

        public string worktime { get; set; }
    }
}
