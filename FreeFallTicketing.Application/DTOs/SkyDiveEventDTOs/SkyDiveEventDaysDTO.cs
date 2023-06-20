namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventDaysDTO : BaseDTO<Guid>
    {
        public SkyDiveEventDaysDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date, int emptyCapacity, int flightQty, int userTicketQty) : base(id, createdAt, updatedAt)
        {
            Date = date;
            EmptyCapacity = emptyCapacity;
            FlightQty = flightQty;
            UserTicketQty = userTicketQty;
        }

        public DateTime Date { get; set; }
        public int EmptyCapacity { get; set; }
        public int FlightQty { get; set; }
        public int UserTicketQty { get; set; }
    }
}
