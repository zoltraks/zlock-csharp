using System;
using System.Runtime.Serialization;

namespace ZLock.Error
{
    [Serializable]
    internal class NotEnoughArguments : Exception
    {
        public NotEnoughArguments() : base("Not enough arguments")
        {
        }
    }
}