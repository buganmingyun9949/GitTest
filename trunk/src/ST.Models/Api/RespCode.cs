using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class RespCode
    {  
        /// <summary>
        /// 
        /// </summary>
        public int retCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string retMsg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public dynamic retData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string retHtml { get; set; }

    }
}
