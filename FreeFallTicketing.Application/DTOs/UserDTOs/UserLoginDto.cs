namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserLoginDto
    {
        public string TokenType { get; internal set; }
        public string AuthToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public int ExpiresIn { get; internal set; }
        public bool IsAdmin { get; set; }
        public bool SecurityInformationCompleted { get; set; }
        public bool PersonalInformationCompleted { get; set; }
    }
}
