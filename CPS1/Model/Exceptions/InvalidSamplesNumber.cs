using System;
using System.Runtime.Serialization;

namespace CPS1.Model.Exceptions
{
    public class InvalidSamplesNumberException : Exception
    {
        public InvalidSamplesNumberException()
        {
        }

        public InvalidSamplesNumberException(string message) : base(message)
        {
        }

        public InvalidSamplesNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSamplesNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}