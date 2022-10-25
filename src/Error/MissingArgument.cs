using System;
using System.Runtime.Serialization;

namespace ZLock.Error
{
    [Serializable]
    internal class MissingArgument : Exception
    {
        public MissingArgument()
        {
        }

        public MissingArgument(string message) : base(message)
        {
        }

        public MissingArgument(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingArgument(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}