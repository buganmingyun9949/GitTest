using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class Card
    {
        /// <summary>
        /// 卡名称  --> 北京九年级
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 卡 ID
        /// </summary>
        public string card_id { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string card_key { get; set; }

        /// <summary>
        /// 当前激活中
        /// </summary>
        public int is_used { get; set; }

        /// <summary>
        /// 当前激活中
        /// </summary>
        public int is_current { get; set; }

        /// <summary>
        /// 绑定用户ID
        /// </summary>
        public string used_user_id { get; set; }

        /// <summary>
        /// 有效期 开始时间
        /// </summary>
        public string used_time { get; set; }

        /// <summary>
        /// 有效期 结束时间
        /// </summary>
        public string expire_time { get; set; }

        /// <summary>
        /// 有效期 结束时间
        /// </summary>
        public string show_expire_time
        {
            get
            {

                if (expire_time == null || string.IsNullOrEmpty(expire_time))
                {
                    return "还未绑定学习卡";
                }
                else
                {
                    return Convert.ToDateTime(expire_time).ToString("yyyy年MM月dd日 HH时mm分 到期");
                } 
            }
        }

        /// <summary>
        /// 有效期(开始时间——结束时间)
        /// </summary>
        public string ctime { get; set; }

        /// <summary>
        /// 当前课程为1，已过期为2，未绑定为0
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 有效期(开始时间——结束时间)
        /// </summary>
        public string Validity { get; set; }

        /// <summary>
        /// 所属年级
        /// </summary>
        public string grade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int agent_id { get; set; }

        /// <summary>
        /// 卡片状态
        /// </summary>
        public int expire_status { get; set; }

        /// <summary>
        /// 卡片状态
        /// </summary>
        public int card_type { get; set; }
    }
}
