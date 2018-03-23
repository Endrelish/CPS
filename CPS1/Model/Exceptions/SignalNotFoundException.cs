namespace CPS1.Model.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class SignalNotFoundException : Exception
    {
        public SignalNotFoundException()
        {
        }

        public SignalNotFoundException(string message)
            : base(message)
        {
        }

        public SignalNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SignalNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}