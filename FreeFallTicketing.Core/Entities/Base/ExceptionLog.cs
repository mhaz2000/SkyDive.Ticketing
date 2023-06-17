namespace SkyDiveTicketing.Core.Entities.Base
{
    public class ExceptionLog : BaseEntity
    {
        public ExceptionLog() : base()
        {

        }

        public static ExceptionLog Create(string requestUrl, string message, string innerExceptionMessage, string stackTrace)
        {
            var newExceptionLog = new ExceptionLog()
            {
                RequestUrl = requestUrl,
                Message = message,
                InnerExceptionMessage = innerExceptionMessage,
                StackTrace = stackTrace,
            };

            return newExceptionLog;
        }

        public string? RequestUrl { get; private set; }
        public string? Message { get; private set; }
        public string? InnerExceptionMessage { get; private set; }
        public string? StackTrace { get; private set; }
    }
}
