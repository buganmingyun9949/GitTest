using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Paper
{
    public class SimulationPaper
    {
        /// <summary>
        /// 模拟题id
        /// </summary>
        public string simulationId { get; set; }
        /// <summary>
        /// 模拟题名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 模拟试卷 序号
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExamListItem> examList { get; set; }
    }

    /// <summary>
    /// 题目类型，choice：听后选择，speak：听后回答，recordAndRelate：听后记录并转述，read：短文朗读
    /// </summary>
    public enum QsType
    {
        choice,
        speak,
        recordAndRelate,
        read
    }
}
