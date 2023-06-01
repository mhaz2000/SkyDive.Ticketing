namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserMessageDTO : BaseDTO<Guid>
    {
        public UserMessageDTO(Guid id, DateTime createdAt, DateTime updatedAt, string text, bool visited) : base(id, createdAt, updatedAt)
        {
            Text = text;
            Visited = visited;
        }

        public string Text { get; set; }
        public bool Visited { get; set; }
    }
}
