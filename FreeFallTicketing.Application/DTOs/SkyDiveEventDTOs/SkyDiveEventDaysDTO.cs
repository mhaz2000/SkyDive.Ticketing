namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventDaysDTO : BaseDTO<Guid>
    {
        public SkyDiveEventDaysDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date, int emptyCapacity, int flightNumber, int ticketNumber) : base(id, createdAt, updatedAt)
        {
            Date = date;
            EmptyCapacity = emptyCapacity;
        }

        public DateTime Date { get; set; }
        public int EmptyCapacity { get; set; }
    }
}
