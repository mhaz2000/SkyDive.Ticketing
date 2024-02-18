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
        public IEnumerable<FlightDTO> Flights { get; set; }
    }

    public class FlightDTO : BaseDTO<Guid>
    {
        public FlightDTO(Guid id, DateTime createdAt, DateTime updatedAt, int flightNumber, int capacity, int voidableQty, string name, string status) : base(id, createdAt, updatedAt)
        {
            FlightNumber = flightNumber;
            Capacity = capacity;
            VoidableQty = voidableQty;
            Status = status;
            Name = name;
        }

        public int FlightNumber { get; set; }
        public int Capacity { get; set; }
        public int VoidableQty { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
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
