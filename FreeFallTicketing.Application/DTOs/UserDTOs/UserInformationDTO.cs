namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserInformationDTO : BaseDTO<Guid>
    {
        public UserInformationDTO(Guid id, DateTime createdAt, DateTime updatedAt, int code, string userName, string mobile, string userStatus, string userType) : base(id, createdAt, updatedAt)
        {
            Code = code;
            UserName = userName;
            Mobile = mobile;
            UserStatus = userStatus;
            UserType = userType;
        }

        public int Code { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string UserStatus { get; set; }
        public string UserType { get; set; }
    }
}
