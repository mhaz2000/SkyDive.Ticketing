using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserDTO : BaseDTO<string>
    {
        public UserDTO(string id, string username, string phone, string email, string firstName, string lastName,
            UserStatus status, int code, string userType, DateTime createdAt, DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            Username = username;
            Phone = phone;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Status= status;
            Code = code;
            UserType = userType;
        }

        public string Username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserStatus Status { get; set; }
        public string UserType { get; set; }
        public int Code { get; set; }
    }
}
