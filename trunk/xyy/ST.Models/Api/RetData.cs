using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class RetData
    {
        #region << 验证码 >>

        /// <summary>
        /// 用户身份，每次请求需要身份认证的接口时需要加入到url参数
        /// </summary>
        public string token { get; set; }

        #endregion

        #region << 登录 >>

        /// <summary>
        /// 学习卡状态 -1未绑定 0 已绑定已失效 1已绑定未失效
        /// </summary>
        public int expire_status { get; set; }
        public object retData { get; set; }

        #endregion

        #region << 卡片 >>

        public string card_id { get; set; }
        public string card_key { get; set; }
        public int is_used { get; set; }
        public int is_current { get; set; }
        public string used_time { get; set; }
        public string used_user_id { get; set; }
        public string expire_time { get; set; }
        public int agent_id { get; set; }
        public string grade { get; set; }
        public string ctime { get; set; }
        public int card_type { get; set; }


        #endregion

        public List<Card> Cards;

    }
}
