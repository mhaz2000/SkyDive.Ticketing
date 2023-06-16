using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserDTO : BaseDTO<Guid>
    {
        public UserDTO(Guid id, string username, string phone, string email, string firstName, string lastName,
            UserStatus status, string statusDisplay, int code, string userType, DateTime createdAt, DateTime updatedAt, string nationalCode, DateTime? birthDate)
            : base(id, createdAt, updatedAt)
        {
            Username = username;
            Phone = phone;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Status = status.ToString();
            Code = code;
            UserType = userType;
            NationalCode = nationalCode;
            BirthDate = birthDate;
            StatusDisplay = statusDisplay;
        }

        public string Username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StatusDisplay { get; set; }
        public string Status { get; set; }
        public string UserType { get; set; }
        public int Code { get; set; }
        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
