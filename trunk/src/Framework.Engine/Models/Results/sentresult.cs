using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Framework.Engine.Models.Results
{
    public class sentresult : BaseNormalResult
    {
        public int textmode { get; set; }

        public int useref { get; set; }

        public int forceout { get; set; }

        public int usehookw { get; set; }

        public int is_en { get; set; }

        public int pron { get; set; }

        public int accuracy { get; set; }

        public int integrity { get; set; }

        public fluency fluency { get; set; }

        public object sentdetails { get; set; }

       //precision:不包含 tips
    }

    public class fluency
    {
        public int overall { get; set; }

        public int pause { get; set; }

        public int speed { get; set;  }
    }

    public class rhythm
    {
        public int overall { get; set; }

        public int sense { get; set; }

        public int stress { get; set; }

        public int tone { get; set; }
    }

}
