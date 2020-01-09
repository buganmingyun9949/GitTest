namespace ST.Common.Domain
{
    public class ErrMsgLog
    {
        public string user { get; set; }
        public string msginfo { get; set; }

        public object error { get; set; }

        public string scoreJson { get; set; }

        public bool isErr { get; set; }
    }
}