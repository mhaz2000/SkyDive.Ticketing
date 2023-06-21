using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(User user) : base()
        {
            User = user;
            ShoppingCartTickets = new List<ShoppingCartTicket>();
        }

        public ICollection<ShoppingCartTicket> ShoppingCartTickets { get; set; }
        public User User { get; set; }
    }
}
