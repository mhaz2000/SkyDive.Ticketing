using Newtonsoft.Json;

namespace SkyDiveTicketing.API.Base
{
    public class ResponseMessage
    {
        public ResponseMessage()
        {

        }

        /// <inheritdoc />
        public ResponseMessage(string message)
        {
            Message = message;
        }

        public ResponseMessage(string message, object content)
        {
            Message = message;
            Content = content;
            Total = 0;
        }

        /// <inheritdoc />
        public ResponseMessage(string message, object content, int total)
        {
            Message = message;
            Content = content;
            Total = total;
        }

        public string Message { get; set; }

        public object Content { get; set; }
        public int Total { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
