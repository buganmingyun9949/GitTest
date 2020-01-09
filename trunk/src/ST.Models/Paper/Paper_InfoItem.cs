using System.Collections.Generic;

namespace ST.Models.Paper
{
    public class Paper_InfoItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string qs_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_keyword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int info_sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int info_repet_times { get; set; }
        /// <summary>
        /// 多次播放 间隔时间 秒
        /// </summary>
        public int info_interval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_content_source_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_content_img_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string answer_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int info_prepare_second { get; set; }
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
        /// 多次播放 间隔 音频
        /// </summary>
        public string item_tip_source_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_content_source_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_content_source_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_content_img { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_del { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Paper_ItemsItem> items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qs_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int qs_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int qs_true_type { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public string qs_show_score_text
        {
            get
            {
                if (qs_show_score == null || qs_show_score < 0)
                {
                    return "未练习";
                }


                return $"{qs_show_score} 分";
            }
        } 

        /// <summary>
        /// 得分
        /// </summary>
        public int? qs_show_score { get; set; }

        /// <summary>
        /// 得分 明细
        /// </summary>
        public string qs_show_score_result { get; set; }
    }

    public class info_content
    {
        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string audio { get; set; }
    }
}