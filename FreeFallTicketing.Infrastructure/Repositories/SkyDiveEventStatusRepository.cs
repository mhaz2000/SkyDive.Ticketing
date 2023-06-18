using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class SkyDiveEventStatusRepository : Repository<SkyDiveEventStatus>, ISkyDiveEventStatusRepository
    {
        public SkyDiveEventStatusRepository(DataContext context) : base(context)
        {
        }

        public async Task Create(string title, bool reservalbe)
        {
            await Context.SkyDiveEventStatuses.AddAsync(new SkyDiveEventStatus(title, reservalbe));
        }

        public void Update(string title, bool reservable, SkyDiveEventStatus status)
        {
            status.Title = title;
            status.Reservable = reservable;
        }
    }
}
