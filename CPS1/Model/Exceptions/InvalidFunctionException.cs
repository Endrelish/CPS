namespace CPS1.Model.Exceptions
{
    using System;
    using System.Runtime.Serialization;

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