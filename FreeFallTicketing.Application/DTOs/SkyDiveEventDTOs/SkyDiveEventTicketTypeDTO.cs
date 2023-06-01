namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventTicketTypeDTO : BaseDTO<Guid>
    {
        public SkyDiveEventTicketTypeDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title, int capacity)
            : base(id, createdAt, updatedAt)
        {
            Title = title;
            Capacity = capacity;
        }

        public string Title { get; set; }
        public int Capacity { get; set; }
    }
}
