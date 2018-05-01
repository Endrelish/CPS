using System;
using System.Runtime.Serialization;

namespace CPS1.Model.Exceptions
{
    public class SignalNotFoundException : Exception
    {
        public SignalNotFoundException()
        {
        }

        public SignalNotFoundException(string message) : base(message)
        {
        }

        public SignalNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SignalNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}