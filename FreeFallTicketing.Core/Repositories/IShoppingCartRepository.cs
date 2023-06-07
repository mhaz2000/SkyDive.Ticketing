using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task AddToShoppingCart(User user, IEnumerable<Ticket> tickets);

        Task ClearShoppingCartAsync(User user);
    }
}
