using SkyDiveTicketing.Application.Base;

namespace SkyDiveTicketing.Application.Helpers
{
    internal class SMSHelpler
    {
        internal static void SendOtp(string receiver, string otpCode, string apiKey, string templateKey)
        {
            try
            {
                var receptor = receiver;
                var api = new Kavenegar.KavenegarApi(apiKey);
                api.VerifyLookup(receptor, otpCode, templateKey);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ManagedException("در ارسال پیامک خطایی رخ داده است.");
            }
        }
    }
}
