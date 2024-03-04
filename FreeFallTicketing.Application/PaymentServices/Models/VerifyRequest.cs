namespace SkyDiveTicketing.Application.PaymentServices.Models
{
    internal record VerifyRequest
    {
        public string Merchant_id { get; set; }
        public int Amount { get; set; }
        public string Authority { get; set; }
    }
}
