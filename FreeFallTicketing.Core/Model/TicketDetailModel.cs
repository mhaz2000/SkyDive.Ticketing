using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Core.Model
{
    public class TicketDetailModel
    {
        public SkyDiveEvent SkyDiveEvent { get; set; }
        public SkyDiveEventItem SkyDiveEventItem { get; set; }
        public FlightLoad FlightLoad { get; set; }
        public FlightLoadItem FlightLoadItem { get; set; }
        public Ticket Ticket { get; set; }
    }
}
