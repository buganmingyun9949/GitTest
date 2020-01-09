using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class ExamDetailListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int examDetailId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 原文
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 原文音频文件名
        /// </summary>
        public string audio { get; set; }

        /// <summary>
        /// 用户答案音频地址
        /// </summary>
        public string userAudio { get; set; }

        /// <summary>
        /// 用户得分
        /// </summary>
        public float userScore { get; set; }

        /// <summary>
        /// 声通评测返回json
        /// </summary>
        public string resJson { get; set; }

        /// <summary>
        /// 填空题图片URL
        /// </summary>
        public string pic { get; set; }

        /// <summary>
        /// 题目集合
        /// </summary>
        public List<QuestionListItem> questionList { get; set; }
    }
}
