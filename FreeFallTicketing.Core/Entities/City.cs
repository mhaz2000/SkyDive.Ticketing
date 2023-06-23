namespace SkyDiveTicketing.Core.Entities
{
    public class DefaultCity
    {
        public DefaultCity()
        {

        }

        public DefaultCity(string province, string state, string city)
        {
            Id = Guid.NewGuid();
            Province = province;
            State = state;
            City = city;
        }

        public Guid Id { get; set; }
        public string? Province { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
