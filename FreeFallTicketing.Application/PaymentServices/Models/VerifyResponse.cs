namespace SkyDiveTicketing.Application.PaymentServices.Models
{
    internal record VerifyResponse
    {
        public VerifyResponseData Data { get; set; }
        public List<string> Errors { get; set; }
    }
    internal record VerifyResponseData
    {
        public int Code { get; set; }
        public int Ref_id { get; set; }
        public string Fee_type { get; set; }
        public int Fee { get; set; }
    }


}
