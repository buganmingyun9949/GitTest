using System.Collections.Generic;

namespace ST.Models.Paper
{

    public class Paper_DetailItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string qs_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paper_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qs_id { get; set; }
        /// <summary>
        /// 第一题  朗读句子。（共5小题，每小题1分，共5分）
        /// </summary>
        public string qs_title { get; set; }
        /// <summary>
        /// 请用规范的英语语音、语调朗读下面五个句子，请先听两遍示范。现在你有10秒钟的时间准备朗读句子，当听到“开始录音”的信号后，请在10秒内朗读句子一遍；当听到要求“停止录音”的信号时，应立即终止做题。
        /// </summary>
        public string qs_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qs_remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int qs_sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string source_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string source_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string source_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qs_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qs_true_type { get; set; }
        /// <summary>
        /// 16、T-朗读句子
        /// </summary>
        public string qs_type_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Paper_InfoItem> info { get; set; }
    }
}