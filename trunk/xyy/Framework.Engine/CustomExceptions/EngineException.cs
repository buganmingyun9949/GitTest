using System;

namespace Framework.Engine.CustomExceptions
{
    public class EngineException : Exception
    {
        public readonly string ErrorMsg;

        public readonly EngineErrorCode ErrorCode;

        public new readonly Exception InnerException;

        public EngineException(string message, EngineErrorCode code)
        {
            ErrorMsg = message;
            ErrorCode = code;
        }

        public EngineException(string message, EngineErrorCode code, Exception inner)
            : this(message, code)
        {
            InnerException = inner;
        }
    }
}
