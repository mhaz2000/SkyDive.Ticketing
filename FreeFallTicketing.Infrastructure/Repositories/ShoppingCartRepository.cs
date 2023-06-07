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

        public async Task AddToShoppingCart(User user, IEnumerable<Ticket> tickets)
        {
            var shoppingCart = await Context.ShoppingCarts.Include(c=>c.User).Where(c => c.User == user).FirstOrDefaultAsync();
            if (shoppingCart is null)
                await Context.ShoppingCarts.AddAsync(new ShoppingCart(tickets, user));
            else
                shoppingCart.Tickets = tickets.ToList();
        }

        public async Task ClearShoppingCartAsync(User user)
        {
            var shoppingCart = await Context.ShoppingCarts.Include(c => c.User).Where(c => c.User == user).FirstOrDefaultAsync();
            if(shoppingCart is not null)
                shoppingCart.Tickets.Clear();

        }
    }
}
