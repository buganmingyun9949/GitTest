using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{

    public class SimulationItem
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


        #region << old >>


        /// <summary>
        /// 模拟题 Id。
        /// </summary>
        public int SimulationId { get; set; }

        /// <summary>
        /// 试卷名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模拟题编号。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 用户得分。
        /// </summary>
        public float UserScore { get; set; }

        /// <summary>
        /// 平均分。
        /// </summary>
        public float AvgScore { get; set; }

        /// <summary>
        /// 平均分（5 分制）。
        /// </summary>
        public float? AvgScore5Points { get; set; }

        /// <summary>
        /// 完成度。
        /// </summary>
        public float CompleteProgress { get; set; }

        /// <summary>
        /// 压缩包地址。
        /// </summary>
        public string ZipFileUrl { get; set; }

        /// <summary>
        /// 压缩包名称。
        /// </summary>
        public string ZipFileName { get; set; }

        /// <summary>
        /// 压缩包大小。
        /// </summary>
        public long ZipSize { get; set; }

        #endregion
    }
}
