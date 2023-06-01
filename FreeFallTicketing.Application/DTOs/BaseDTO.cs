namespace SkyDiveTicketing.Application.DTOs
{
    public abstract class BaseDTO<TIdType>
    {
        public BaseDTO(TIdType id, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public TIdType Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
