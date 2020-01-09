using ST.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_App.Common
{
    public class BackgroundTask<T>
    {

        public T Model { get; set; }

        public ApiType ApiType { get; set; }
    }
}
