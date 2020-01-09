using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class AnswerListItem
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 用户答案,未作答为空字符串
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string userText { get; set; }

        /// <summary>
        /// audio
        /// </summary>
        public string audio { get; set; }

        /// <summary>
        /// 用户得分
        /// </summary>
        public float userScore { get; set; }
    }
}
