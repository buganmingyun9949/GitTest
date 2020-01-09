using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class Result
    {
        /// <summary>
        /// 我的总分
        /// </summary>
        public float userTotalScore { get; set; }
        /// <summary>
        /// 题目总分
        /// </summary>
        public float totalScore { get; set; }

        /// <summary>
        /// 题目
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 题目类型
        /// </summary>
        public string completeTime { get; set; }
        /// <summary>
        /// 题目类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 清单
        /// </summary>
        public List<ExamDetailListItem> examDetailList { get; set; }
    }
}
