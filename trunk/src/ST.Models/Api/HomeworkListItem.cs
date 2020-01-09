using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{

    public class HomeworkListItem
    {
        /// <summary>
        /// 模拟题 Id。
        /// </summary>
        public string exam_id { get; set; }
        /// <summary>
        /// 模拟题 Id。
        /// </summary>
        public string exam_attend_id { get; set; }
        /// <summary>
        /// 试卷序号。
        /// </summary>
        public int exam_sort { get; set; }

        /// <summary>
        /// 作业标题。
        /// </summary>
        public string exam_title { get; set; }

        /// <summary>
        /// 试卷标题。
        /// </summary>
        public string paper_title { get; set; }

        /// <summary>
        /// 试卷总分。
        /// </summary>
        public float? paper_score { get; set; } = 0.0F;

        /// <summary>
        /// 我的当前得分。
        /// </summary>
        public float? score { get; set; } = 0.0F;

        /// <summary>
        /// 作业状态，100：未开始，101：已开始， 201已完成
        /// </summary>
        public int? status { get; set; } = 0;

        /// <summary>
        /// 考试进度
        /// 完成进度，保留2位小数
        /// </summary>
        public float? exam_process { get; set; } = 0.00F;

        /// <summary>
        /// 开始
        /// </summary>
        public string start_at { get; set; }

        /// <summary>
        /// 完成
        /// </summary>
        public string ended_at { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string publish_time { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public string finish_time { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime publishTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime finishTime { get; set; }

        /// <summary>
        /// 开始
        /// </summary>
        public DateTime startat { get; set; }

        /// <summary>
        /// 完成
        /// </summary>
        public DateTime endedat { get; set; }
    }
}
