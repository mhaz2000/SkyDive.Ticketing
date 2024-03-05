using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.PaymentServices.Models;
using System.Text;

namespace SkyDiveTicketing.Application.PaymentServices
{
    public class ZarinpalPaymentService : IZarinpalPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUrl;
        private readonly string _merchantId;
        private readonly string _callBackUrl;

        private readonly string _gatewayUrl;

        public ZarinpalPaymentService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _requestUrl = configuration["Zarinpal:RequestUrl"] ?? string.Empty;
            _merchantId = configuration["Zarinpal:MerchantId"] ?? string.Empty;
            _callBackUrl = configuration["Zarinpal:CallBackUrl"] ?? string.Empty;

            _gatewayUrl = configuration["Zarinpal:GatewayUrl"] ?? string.Empty;
        }


        public async Task<string> Checkout(double amount)
        {
            var paymentRequest = new PaymentRequest()
            {
                Amount = (int)amount,
                Callback_url = _callBackUrl,
                Merchant_id = _merchantId,
                Description = "بابت تسویه سبد خرید باشگاه سقوط آزاد ایرانیان"
            };

            var paymentResponse = await PaymentRequestAsync(paymentRequest).ConfigureAwait(false);

            if (paymentResponse is null || paymentResponse.Data.Code != 100)
                throw new ManagedException("در فرایند انتقال به درگاه پرداخت خطایی رخ داده است.");

            return $"{_gatewayUrl}/{paymentResponse.Data.Authority}";
        }

        public async Task<ulong> Verify(string authority, double amount)
        {
            var verifyRequest = new VerifyRequest()
            {
                Authority = authority,
                Merchant_id = _merchantId,
                Amount = (int)amount
            };

            var verifyResponse = await VerifyRequestAsync(verifyRequest).ConfigureAwait(false);

            if (verifyResponse is null)
                throw new ManagedException("در فرایند پرداخت خطایی رخ داده است.");

            if (verifyResponse.Data.Code != 100 && verifyResponse.Data.Code != 101)
                throw new ManagedException(string.Join("\n", verifyResponse.Errors));

            return verifyResponse.Data.Ref_id;
        }

        private async Task<PaymentResponse?> PaymentRequestAsync(PaymentRequest paymentRequest)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() } }; 

            HttpResponseMessage response = await _httpClient.PostAsync($"{_requestUrl}/request.json",
                new StringContent(JsonConvert.SerializeObject(paymentRequest, settings), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PaymentResponse>(content);
        }

        private async Task<VerifyResponse?> VerifyRequestAsync(VerifyRequest paymentRequest)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() } };

            HttpResponseMessage response = await _httpClient.PostAsync($"{_requestUrl}/verify.json",
                new StringContent(JsonConvert.SerializeObject(paymentRequest, settings), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<VerifyResponse>(content);
        }
    }
}
