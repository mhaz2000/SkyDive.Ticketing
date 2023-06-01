namespace SkyDiveTicketing.Application.DTOs
{
    public class CityDTO
    {
        public CityDTO(Guid id, string province, string state, string city)
        {
            Id = id;
            Province = province;
            State = state;
            City = city;
        }

        public Guid Id { get; set; }
        public string Province { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
