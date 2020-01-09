using System.Collections.Generic;

namespace ST.Models.Paper
{
    public class Paper_Info
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Paper_DetailItem> paper_detail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int question_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paper_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public dynamic paper_assets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public Paper_assets paper_assets { get; set; }
    }
}