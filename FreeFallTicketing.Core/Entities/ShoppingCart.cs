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
            Items= new List<ShoppingCartItem>();
        }

        public ICollection<ShoppingCartItem> Items { get; set; }
        public User User { get; set; }
    }

    public class ShoppingCartItem : BaseEntity
    {
        public ShoppingCartItem()
        {

        }

        public ShoppingCartItem(FlightLoadItem flightLoadItem, int qty)
        {
            FlightLoadItem = flightLoadItem;
            Qty = qty;
        }

        public FlightLoadItem FlightLoadItem { get; set; }
        public int Qty { get; set; }
    }

}
