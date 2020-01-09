using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class CardsListItem
    {
        /// <summary>
        /// 卡名称  --> 北京九年级
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string card_id { get; set; }

        /// <summary>
        /// 当前激活中
        /// </summary>
        public string is_current { get; set; }

        /// <summary>
        /// 有效期 开始时间
        /// </summary>
        public string used_time { get; set; }

        /// <summary>
        /// 有效期 结束时间
        /// </summary>
        public string expire_time { get; set; }

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
        /// 卡片状态
        /// </summary>
        public int expire_status { get; set; }
    }
}
