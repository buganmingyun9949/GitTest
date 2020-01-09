using System.Collections.Generic;

namespace ST.Models.Paper
{
    public class Paper_ItemsItem
    {

        /// <summary>
        /// 
        /// </summary>
        public string item_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string info_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_keyword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float item_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int item_prepare_second { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int item_answer_second { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int item_repet_times { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string c_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string u_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_del { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string source_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string img_source_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string video_source_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_remark { get; set; }
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
        public string img_source_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string video_source_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Paper_AnswersItem> answers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int item_no { get; set; }
    }
}