namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventStatusDTO : BaseDTO<Guid>
    {
        public SkyDiveEventStatusDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title, bool reservable) : base(id, createdAt, updatedAt)
        {
            Title = title;
            Reservable = reservable;
        }

        public string Title { get; set; }
        public bool Reservable { get; set; }
    }
}
