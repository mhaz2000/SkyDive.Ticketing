namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserPersonalInformationDTO : BaseDTO<Guid>
    {
        public UserPersonalInformationDTO(Guid id, DateTime createdAt, DateTime updatedAt, string nationalCode, DateTime? birthDate, string firstName,
            string lastName, string email, Guid? cityId, string state, string city, string address, float? weight, float? height) : base(id, createdAt, updatedAt)
        {
            NationalCode = nationalCode;
            BirthDate = birthDate;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CityId = cityId;
            State = state;
            City = city;
            Address = address;
            Weight = weight;
            Height = height;
        }

        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid? CityId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
    }
}
