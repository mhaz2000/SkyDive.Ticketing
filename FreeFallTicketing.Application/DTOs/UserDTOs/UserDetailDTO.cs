using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserDetailDTO : BaseDTO<Guid>
    {
        public UserDetailDTO(Guid id, DateTime createdAt, DateTime updatedAt, string username, string firstName, string lastName,
            string userTypeDisplay, Guid userTypeId, string nationalCode, DateTime? birthDate, string city, Guid? cityId, string address,
            int userCode, string email, string phone, float? height, float? weight, UserStatus userStatus, string userStatusDisplay,
            string emergencyContact, string emergencyPhone) : base(id, createdAt, updatedAt)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            UserTypeDisplay = userTypeDisplay;
            UserTypeId = userTypeId;
            NationalCode = nationalCode;
            BirthDate = birthDate;
            City = city;
            CityId = cityId;
            Address = address;
            Email = email;
            Phone = phone;
            UserStatus = userStatus;
            UserStatusDisplay = userStatusDisplay;
            Height = height;
            Weight = weight;
            UserCode = userCode;
            EmergencyPhone = emergencyPhone;
            EmergencyContact = emergencyContact;
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserTypeDisplay { get; set; }
        public Guid UserTypeId { get; set; }
        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public Guid? CityId { get; set; }
        public string Address { get; set; }
        public int UserCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public UserStatus UserStatus { get; set; }
        public string UserStatusDisplay { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
    }
}
