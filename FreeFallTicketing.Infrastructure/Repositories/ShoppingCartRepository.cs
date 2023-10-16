using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(DataContext context) : base(context)
        {
        }

        public async Task AddToShoppingCart(User user, List<Tuple<FlightLoadItem, User>> flightLoadItems, SkyDiveEvent skyDiveEvent)
        {
            var shoppingCart = await Context.ShoppingCarts.Include(c => c.User).Include(c => c.Items).Where(c => c.User == user).FirstOrDefaultAsync();
            if (shoppingCart is null)
            {
                var entity = new ShoppingCart(user, skyDiveEvent);

                foreach (var flightLoadItem in flightLoadItems)
                {
                    var shoppingCartItem = new ShoppingCartItem(flightLoadItem.Item1, flightLoadItem.Item2);

                    Context.ShoppingCartItems.Add(shoppingCartItem);

                    entity.Items.Add(shoppingCartItem);
                }

                await Context.ShoppingCarts.AddAsync(entity);
            }
            else
            {

                foreach (var flightLoadItem in flightLoadItems)
                {
                    var shoppingCartItem = new ShoppingCartItem(flightLoadItem.Item1, flightLoadItem.Item2);

                    shoppingCart.Items.Add(shoppingCartItem);
                    shoppingCart.SkyDiveEvent = skyDiveEvent;

                    Context.ShoppingCartItems.Add(shoppingCartItem);
                }
            }
        }

        public async Task ClearShoppingCartAsync(User user)
        {
            var shoppingCart = await Context.ShoppingCarts.Include(c => c.User).Include(c=> c.SkyDiveEvent).Include(c => c.Items)
                .Where(c => c.User == user).FirstOrDefaultAsync();

            if (shoppingCart is not null)
            {
                Context.ShoppingCartItems.RemoveRange(shoppingCart.Items);
                shoppingCart.SkyDiveEvent = null;
                shoppingCart.Items.Clear();
            }
        }

        public async Task<ShoppingCart?> GetUserShoppingCart(User user)
        {
            return await Context.ShoppingCarts
                .Include(c => c.User)
                .Include(c => c.SkyDiveEvent).ThenInclude(c => c.TypesAmount)
                .Include(c => c.Items).ThenInclude(c => c.ReservedFor)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoadItem).ThenInclude(c => c.FlightLoadType)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoadItem).ThenInclude(c => c.Tickets).ThenInclude(c => c.ReservedBy)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoadItem).ThenInclude(c => c.Tickets).ThenInclude(c => c.LockedBy)
                .FirstOrDefaultAsync(c => c.User == user);
        }

        public async Task RemoveUserShoppingCart(User user)
        {
            var shoppingCart = await Context.ShoppingCarts
                .Include(c => c.User)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.User == user);

            if (shoppingCart is not null)
            {
                Context.ShoppingCartItems.RemoveRange(shoppingCart.Items);
                Context.ShoppingCarts.Remove(shoppingCart);
            }

        }
    }
}
