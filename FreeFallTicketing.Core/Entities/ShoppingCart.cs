using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart(Ticket ticket, User user) : base()
        {
            User = user;
            Ticket = ticket;
        }

        public Ticket Ticket { get; set; }
        public User User { get; set; }
    }
}
