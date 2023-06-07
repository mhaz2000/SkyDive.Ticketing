namespace SkyDiveTicketing.API.Extensions
{
    public static class HttpRequestHelper
    {
        public static string GetAccessToken(this HttpRequest httpRequest) => httpRequest.Headers.ContainsKey("Authorization")
                                                                        ? httpRequest.Headers["Authorization"].ToString().Split(" ")[1]
                                                                        : string.Empty;
    }
}
