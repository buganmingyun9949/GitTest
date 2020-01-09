using System;

namespace Framework.Engine.Models.Results
{
    [Serializable]
    public class predresult : BaseNormalResult
    {
        public predDetails[] details { get; set; }

        public double fluency { get; set; }

        public double integrity { get; set; }

        public double pron { get; set; }

        public int rhythm { get; set; }
    }

    [Serializable]
    public class predDetails
    {
        //ADD
        public int end { get; set; }

        //ADD
        public int start { get; set; }

        public double score { get; set; }

        //update
        public string text { get; set; }
    }
}
