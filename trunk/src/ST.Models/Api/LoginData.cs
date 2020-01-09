using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class LoginData : RetData
    {
        /// <summary>
        /// 学习卡状态 -1未绑定 0 已绑定已失效 1已绑定未失效
        /// </summary>
        public int expire_status { get; set; }
        public object retData { get; set; }
    }
}
