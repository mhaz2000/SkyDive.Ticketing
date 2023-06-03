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
    }
}
