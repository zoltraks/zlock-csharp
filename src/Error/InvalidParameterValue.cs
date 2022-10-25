using System;
using System.Runtime.Serialization;

namespace ZLock.Error
{
    [Serializable]
    internal class InvalidParameterValue : Exception
    {
        public InvalidParameterValue(string parameter, string value) : base(Format(parameter, value)) { }

        private static string Format(string parameter, string value) => string.Format("Invalid value \"{0}\" for parameter \"{1}\"", value, parameter);
    }
}