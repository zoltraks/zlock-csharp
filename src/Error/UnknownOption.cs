using System;
using System.Runtime.Serialization;

namespace ZLock.Error
{
    [Serializable]
    internal class UnknownOption : Exception
    {
        public UnknownOption(string message) : base(Format(message)) { }

        private static string Format(string message) => string.Format("Unknown option {0}", message);
    }
}