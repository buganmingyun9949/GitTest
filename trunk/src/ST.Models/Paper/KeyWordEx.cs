namespace ST.Models.Paper
{
    public class KeyWordEx
    {
        /// <summary>
        /// 音标
        /// </summary>
        public string yb { get; set; }
        /// <summary>
        /// 单词 中文
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 样例 中文
        /// </summary>
        public string ex_zh { get; set; }
        /// <summary>
        /// 样例 英文
        /// </summary>
        public string ex_en { get; set; }

        /// <summary>
        /// 是否 按照字母显示   为空 null/"" 按字幕显示, 不为空 长度>0 按单词显示
        /// </summary>
        public string sp { get; set; }
    }
}