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
        public string info_sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int info_repet_times { get; set; }
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
    }
}