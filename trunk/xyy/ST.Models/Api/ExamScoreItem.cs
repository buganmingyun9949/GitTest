using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class ExamScoreItem
    {
        /// <summary>
        /// 我的成绩
        /// </summary>
        public float userScore { get; set; }

        /// <summary>
        /// 题目类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 题目总分
        /// </summary>
        public float totalScore { get; set; }
    }
}
