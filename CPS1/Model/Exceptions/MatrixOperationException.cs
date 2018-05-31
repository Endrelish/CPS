using System;
using System.Runtime.Serialization;

namespace CPS1.Model.Exceptions
{
    public class MatrixOperationException : Exception
    {
        public MatrixOperationException()
        {
        }

        public MatrixOperationException(string message) : base(message)
        {
        }

        public MatrixOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MatrixOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}