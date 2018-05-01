using System;
using System.Runtime.Serialization;

namespace CPS1.Model.Exceptions
{
    public class InvalidFunctionException : Exception
    {
        public InvalidFunctionException()
        {
        }

        public InvalidFunctionException(string message) : base(message)
        {
        }

        public InvalidFunctionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidFunctionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}