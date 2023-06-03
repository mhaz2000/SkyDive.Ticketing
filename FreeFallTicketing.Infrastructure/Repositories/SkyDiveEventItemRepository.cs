using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class SkyDiveEventItemRepository : Repository<SkyDiveEventItem>, ISkyDiveEventItemRepository
    {
        public SkyDiveEventItemRepository(DataContext context) : base(context)
        {
        }

        public IQueryable<SkyDiveEventItem> IncludeAll(Expression<Func<SkyDiveEventItem, bool>> predicate)
        {
            return Context.Set<SkyDiveEventItem>()
                .Include(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.Tickets).ThenInclude(c => c.ReservedBy);
        }
    }
}
