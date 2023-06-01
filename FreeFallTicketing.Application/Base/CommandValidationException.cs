using System.Runtime.Serialization;

namespace SkyDiveTicketing.Application.Base
{
    [Serializable]
    public class CommandValidationException : Exception
    {
        public CommandValidationException()
        {
        }

        public CommandValidationException(string message) : base(message)
        {
        }

        public CommandValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ManagedException : Exception
    {
        public ManagedException()
        {
        }

        public ManagedException(string message) : base(message)
        {
        }

        public ManagedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ManagedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    [Serializable]
    public class LoopException : Exception
    {
        public LoopException()
        {
        }

        public LoopException(string message) : base(message)
        {
        }
    }
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
