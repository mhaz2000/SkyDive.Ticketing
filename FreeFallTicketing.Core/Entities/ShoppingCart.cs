using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(IEnumerable<Ticket> tickets, User user) : base()
        {
            User = user;
            Tickets = tickets.ToList();
        }

        public ICollection<Ticket> Tickets { get; set; }
        public User User { get; set; }
    }
}
