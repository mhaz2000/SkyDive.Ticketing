namespace SkyDiveTicketing.Application.PaymentServices.Models
{
    internal record PaymentRequest
    {
        public string Merchant_id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Callback_url { get; set; }
    }
}
