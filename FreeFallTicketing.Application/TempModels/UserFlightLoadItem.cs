using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.TempModels
{
    public class UserFlightLoadItem
    {
        public UserFlightLoadItem()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public FlightLoadItem FlightLoadItem { get; set; }
        public User User { get; set; }
    }
}
