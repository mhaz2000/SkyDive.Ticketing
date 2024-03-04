namespace SkyDiveTicketing.Application.PaymentServices.Models
{
    internal record PaymentResponse
    {
        public PaymentResponseData Data { get; set; }
        public List<string> Errors { get; set; }
    }

    internal record PaymentResponseData
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Authority { get; set; }
        public int Fee{ get; set; }
    }
}
