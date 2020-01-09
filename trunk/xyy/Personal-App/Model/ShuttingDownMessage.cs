using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_App.Model
{class ShuttingDownMessage
    {
        public string CurrentViewName { get; private set; }
        public ShuttingDownMessage(string currentViewName)
        {
            this.CurrentViewName = currentViewName;
        }
    }
}
