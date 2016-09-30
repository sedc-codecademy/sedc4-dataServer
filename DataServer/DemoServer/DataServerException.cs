using System;
using System.Runtime.Serialization;

namespace DemoServer
{
    [Serializable]
    internal class DataServerException : ApplicationException
    {
        public DataServerException()
        {
        }

        public DataServerException(string message) : base(message)
        {
        }

        public DataServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}