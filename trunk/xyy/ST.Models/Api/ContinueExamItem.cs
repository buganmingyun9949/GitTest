using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class ContinueExamItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string examId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string examDetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string questionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BlankAnswerItem> fillBlankAnswerList { get; set; }
    }
}
