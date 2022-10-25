using System;

namespace ZLock.Error
{
    [Serializable]
    internal class MissingParameterValue : Exception
    {
        public MissingParameterValue(string message) : base(Format(message)) { }

        private static string Format(string message) => string.Format("Missing expected value for parameter \"{0}\"", message);
    }
}