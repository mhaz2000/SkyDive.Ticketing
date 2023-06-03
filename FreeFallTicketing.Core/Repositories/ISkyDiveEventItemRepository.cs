using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISkyDiveEventItemRepository : IRepository<SkyDiveEventItem>
    {
        IQueryable<SkyDiveEventItem> IncludeAll(Expression<Func<SkyDiveEventItem, bool>> predicate);
    }
}
