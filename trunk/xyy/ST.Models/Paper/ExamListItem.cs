using System.Collections.Generic;
using ST.Models.Api;

namespace ST.Models.Paper
{
    public class ExamListItem
    {
        /// <summary>
        /// 题目id
        /// </summary>
        public string examId { get; set; }
        /// <summary>
        /// 题目总分
        /// </summary>
        public string totalScore { get; set; }
        /// <summary>
        /// 题目类型，choice：听后选择，speak：听后回答，recordAndRelate：听后记录并转述，read：短文朗读
        /// </summary>
        public QsType type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExamDetailListItem> examDetailList { get; set; }
    }
}