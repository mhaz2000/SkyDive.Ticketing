namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventItemDTO : BaseDTO<Guid>
    {
        public SkyDiveEventItemDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date) : base(id, createdAt, updatedAt)
        {
            Date = date;
            Flights = new List<FlightDTO>();
        }

        public DateTime Date { get; set; }
        public int TotalAvailableTicketsNumber => Flights.Sum(flight => flight.Tickets.Sum(ticket => ticket.AvailableTicketsNumber));
        public IEnumerable<FlightDTO> Flights { get; set; }
    }

    public class FlightDTO : BaseDTO<Guid>
    {
        public FlightDTO(Guid id, DateTime createdAt, DateTime updatedAt, int flightNumber) : base(id, createdAt, updatedAt)
        {
            FlightNumber = flightNumber;
            Tickets = new List<TicketDTO>();
        }

        public int FlightNumber { get; set; }
        public IEnumerable<TicketDTO> Tickets { get; set; }
    }

    public class TicketDTO : BaseDTO<Guid>
    {
        public TicketDTO(Guid id, DateTime createdAt, DateTime updatedAt, string type, double amount, int availableTicketsNumber) : base(id, createdAt, updatedAt)
        {
            Amount = amount;
            AvailableTicketsNumber = availableTicketsNumber;
            Type = type;
        }

        public string Type { get; set; }
        public double Amount { get; set; }
        public int AvailableTicketsNumber { get; set; }
    }
}
