using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Message : BaseEntity
    {
        public Message()
        {

        }
        public Message(string text) : base()
        {
            Text = text;
            Visited= false;
        }

        public string Text { get; set; }
        public bool Visited { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
