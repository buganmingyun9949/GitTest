using System;

namespace Framework.Engine
{
    [Serializable]
    public abstract class BaseNormalResult:BaseResult
    {
        public double overall { get; set; }
        public double precision { get; set; }
        public int rank { get; set; }
        public string tips { get; set; }
    }

    [Serializable]
    public class resultInfo
    {
        public int clip { get; set; }

        public int snr { get; set; }

        public int tipId { get; set; }

        public int volume { get; set; }
    }
}
