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
            var shoppingCart = await Context.ShoppingCarts.Include(c => c.User).Include(c => c.ShoppingCartTickets).Where(c => c.User == user).FirstOrDefaultAsync();
            if (shoppingCart is null)
            {
                var entity = new ShoppingCart(user);

                foreach (var ticket in tickets)
                {
                    var shoppingCartTicket = new ShoppingCartTicket()
                    {
                        ShoppingCart = entity,
                        ShoppingCartId = entity.Id,
                        Ticket = ticket,
                        TicketId = ticket.Id
                    };

                    ticket.ShoppingCartTickets.Add(shoppingCartTicket);

                    Context.ShoppingCartTickets.Add(shoppingCartTicket);
                }
                await Context.ShoppingCarts.AddAsync(entity);
            }
            else
            {
                foreach (var ticket in tickets)
                {
                    var shoppingCartTicket = new ShoppingCartTicket()
                    {
                        ShoppingCart = shoppingCart,
                        ShoppingCartId = shoppingCart.Id,
                        Ticket = ticket,
                        TicketId = ticket.Id
                    };

                    Context.ShoppingCartTickets.Add(shoppingCartTicket);
                }
            }
        }

        public async Task ClearShoppingCartAsync(User user)
        {
            var shoppingCart = await Context.ShoppingCarts.Include(c => c.User).Include(c => c.ShoppingCartTickets)
                .Where(c => c.User == user).FirstOrDefaultAsync();

            if (shoppingCart is not null)
            {
                Context.ShoppingCartTickets.RemoveRange(shoppingCart.ShoppingCartTickets);
                shoppingCart.ShoppingCartTickets.Clear();
            }
        }
    }
}
