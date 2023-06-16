using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserInformationDTO : BaseDTO<Guid>
    {
        public UserInformationDTO(Guid id, DateTime createdAt, DateTime updatedAt, int code, string userName, string mobile,
            string userStatusDisplay, UserStatus userStatus, string userType, string firstName, string lastName) : base(id, createdAt, updatedAt)
        {
            Code = code;
            UserName = userName;
            Mobile = mobile;
            UserStatus = userStatus.ToString();
            UserStatusDisplay = userStatusDisplay;
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Code { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string UserStatus { get; set; }
        public string UserStatusDisplay { get; set; }
        public string UserType { get; set; }
    }
}
