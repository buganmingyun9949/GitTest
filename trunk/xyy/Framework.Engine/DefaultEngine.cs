using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine
{
    public class DefaultEngine : BaseEngine
    {
        public DefaultEngine(string appKey, string secretKey, string serverUri, string serverTimeout)
            : base(appKey, secretKey, serverUri, serverTimeout)
        {
            
        }
    }
}
