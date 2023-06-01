using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class CityRepository : Repository<DefaultCity>, ICityRepository
    {
        public CityRepository(DataContext context) : base(context)
        {
        }
    }
}
