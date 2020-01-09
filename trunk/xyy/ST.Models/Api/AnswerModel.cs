using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class AnswerModel
    {
        public string examId { get; set; }
        public string startTime { get; set; }
        public List<AnswerList> answerList { get; set; }
    }
    public class AnswerList
    {
        public string questionId { get; set; }
        public string answerOrder { get; set; }
        public string resJson { get; set; }
        public string orderNum { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }
}
