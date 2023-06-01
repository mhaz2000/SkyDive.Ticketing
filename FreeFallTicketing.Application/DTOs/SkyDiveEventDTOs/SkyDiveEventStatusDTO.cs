namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventStatusDTO : BaseDTO<Guid>
    {
        public SkyDiveEventStatusDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title) : base(id, createdAt, updatedAt)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
