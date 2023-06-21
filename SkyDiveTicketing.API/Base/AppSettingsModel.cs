using SkyDiveTicketing.Application.Base;

namespace SkyDiveTicketing.API.Base
{
    public class AppSettingsModel
    {
        public JwtIssuerOptionsModel JwtIssuerOptions { get; set; }
        public string KavenegarApiKey { get; set; }
        public string VerificationTemplateKey { get; set; }
    }
}
