using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task AddToShoppingCart(User user, IDictionary<FlightLoadItem, int> flightLoadItems);

        Task<ShoppingCart?> GetUserShoppingCart(User user);

        Task ClearShoppingCartAsync(User user);
    }
}
