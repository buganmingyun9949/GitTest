using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{

    public class QuestionListItem
    {
        /// <summary>
        /// 问题id
        /// </summary>
        public string questionId { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string beginText { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public float score { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public float userScore { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string userDuration { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        public string userPronunciation { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        public string userCoherence { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        public string userFluency { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        public string userIntegrity { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        public string userSpeed { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ChoiceItem> choice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int answerOrder { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string answer { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string userAnswer { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string userAudio { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string userText { get; set; }

        /// <summary>
        /// 答案列
        /// </summary>
        public List<AnswerListItem> answerList { get; set; }
    }
}
