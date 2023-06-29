namespace SkyDiveTicketing.Application.DTOs.FlightLoadDTOs
{
    public class FlightLoadItemTicketDTO : BaseDTO<Guid>
    {
        public FlightLoadItemTicketDTO(Guid id, DateTime createdAt, DateTime updatedAt, string ticketNumber,
            string ticketType, bool reservable, string status, Guid ticketTypeId) : base(id, createdAt, updatedAt)
        {
            TicketNumber = ticketNumber;
            TicketType = ticketType;
            Reservable = reservable;
            Status = status;
            TicketTypeId = ticketTypeId;
        }

        public Guid TicketTypeId { get; set; }
        public string TicketNumber { get; set; }
        public string TicketType { get; set; }
        public bool Reservable { get; set; }
        public string Status { get; set; }
    }
}
