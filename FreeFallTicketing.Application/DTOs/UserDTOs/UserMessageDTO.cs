namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserMessageDTO : BaseDTO<Guid>
    {
        public UserMessageDTO(Guid id, DateTime createdAt, DateTime updatedAt, string text, bool visited, string title) : base(id, createdAt, updatedAt)
        {
            Text = text;
            Visited = visited;
            Title = title;
        }

        public string Text { get; set; }
        public string Title { get; set; }
        public bool Visited { get; set; }
    }
}
