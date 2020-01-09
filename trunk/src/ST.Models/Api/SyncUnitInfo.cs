using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class TrainInfo
    {
        /// <summary>
        /// 题目/题型 ID
        /// </summary>
        public string Qs_Type_Id { get; set; }

        /// <summary>
        /// 题目/题型 名称
        /// </summary>
        public string Qs_Type_Name { get; set; }

        /// <summary>
        /// 题目/题型 数量
        /// </summary>
        public string paper_num { get; set; }

    }

    public class Qs_Type_Name
    {
        /// <summary>
        /// 题目/题型 名称
        /// </summary>
        public string Name { get; set; } 
    }
}
