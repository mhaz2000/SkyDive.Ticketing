namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserPersonalInformationDTO : BaseDTO<Guid>
    {
        public UserPersonalInformationDTO(Guid id, DateTime createdAt, DateTime updatedAt, string nationalCode, DateTime? birthDate, string firstName,
            string lastName, string email, string? cityAndState, string address, float? weight, float? height, string emergencyContact,
            string emergencyPhone) : base(id, createdAt, updatedAt)
        {
            NationalCode = nationalCode;
            BirthDate = birthDate;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CityAndState = cityAndState;
            Address = address;
            Weight = weight;
            Height = height;
            EmergencyContact = emergencyContact;
            EmergencyPhone = emergencyPhone;
        }

        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? CityAndState { get; set; }
        public string Address { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
    }
}
