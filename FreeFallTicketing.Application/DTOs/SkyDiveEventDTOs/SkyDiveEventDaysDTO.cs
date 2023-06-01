namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventDaysDTO : BaseDTO<Guid>
    {
        public SkyDiveEventDaysDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date, int capacity) : base(id, createdAt, updatedAt)
        {
            Date = date;
            Capacity = capacity;
        }

        public DateTime Date { get; set; }
        public int Capacity { get; set; }
    }
}
