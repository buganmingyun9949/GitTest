using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Score
{
    public class ScoreJsonResult
    {
    }

    public class Result
    {
        /// <summary>
        /// 
        /// </summary>
        public double fluency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float overall { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double pronunciation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double integrity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string kernel_version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WarningItem> warning { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string resource_version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double speed { get; set; }


        /// <summary>
        /// choice 题型  返回的 匹配值  相似度
        /// </summary>
        public double confidence { get; set; }
        public double coherence { get; set; }

        public dynamic sentences { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string recognition { get; set; }
    }

    public class App
    {
        /// <summary>
        /// 
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string clientId { get; set; }
    }

    public class Request
    {
        /// <summary>
        /// 
        /// </summary>
        public string coreType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string keywords { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string tokenId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string refText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int qType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int qClass { get; set; }
    }

    public class Audio
    {
        /// <summary>
        /// 
        /// </summary>
        public int sampleRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int channel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int complexity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int sampleBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string compress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int quality { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string audioType { get; set; }
    }

    public class Params
    {
        /// <summary>
        /// 
        /// </summary>
        public App app { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Request request { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Audio audio { get; set; }
    }

    public class ScoreRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string tokenId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string recordId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string dtLastResponse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int eof { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Params param
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string refText { get; set; }
    }

    public class SentencesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public float overall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int end { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DetailsItem> details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sentence { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int start { get; set; }
    }

    public class DetailsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int prominence { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int charType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float overall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string word { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int start { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int end { get; set; }
    }
    public class WarningItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    }
}
