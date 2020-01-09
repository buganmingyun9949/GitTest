using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine
{
    public class BaseResult
    {
        public int delaytime { get; set; }

        public int systime { get; set; }

        public int wavetime { get; set; }

        public string version { get; set; }

        public string res { get; set; }

        public resultInfo info { get; set; }

        public int pretime { get; set; }
    }
}
