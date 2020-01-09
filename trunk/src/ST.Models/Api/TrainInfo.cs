using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class SyncUnitInfo
    {
        /// <summary>
        /// unit ID
        /// </summary>
        public string unit_id { get; set; }

        /// <summary>
        /// 书册 书籍ID
        /// </summary>
        public string book_id { get; set; }

        /// <summary>
        /// unit Name
        /// </summary>
        public string unit_name { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string c_time { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public string is_del { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string unit_pic { get; set; }


        /// <summary>
        /// 名字 名称
        /// </summary>
        public string unit_short_name { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string unit_desc { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        public string unit_icon { get; set; }


        /// <summary>
        /// 内容数量
        /// </summary>
        public string paper_num { get; set; }


    }
}
