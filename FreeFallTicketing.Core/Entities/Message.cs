using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Message : BaseEntity
    {
        public Message()
        {

        }
        public Message(string text, string title) : base()
        {
            Text = text;
            Visited = false;
            Title = title;
        }

        public string Text { get; set; }
        public bool Visited { get; set; }
        public string Title { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
