using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Logging;

namespace Framework.Recorder
{
    public class RecordException : Exception
    {
        public RecordException(string message)
            : base(message)
        {
            Log4NetHelper.Error(message);
        }

        public RecordException(string message, Exception e)
            : base(message, e)
        {
            Log4NetHelper.Error(message, e);
        }
    }
}
