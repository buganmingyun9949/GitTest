using Framework.Engine.Models.MetaParameters;

namespace Framework.Engine.Models.BaseParameters
{
    class baserequest
    {
        public string coreType { get; set; }
        public basereftext refText { get; set; }
        public int rank { get; set; }
        public double precision { get; set; }
        public client_params client_params { get; set; }
        public result result { get; set; }
    }
}
