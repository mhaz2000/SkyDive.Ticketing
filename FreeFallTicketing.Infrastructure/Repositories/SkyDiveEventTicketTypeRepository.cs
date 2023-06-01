using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class SkyDiveEventTicketTypeRepository : Repository<SkyDiveEventTicketType>, ISkyDiveEventTicketTypeRepository
    {
        public SkyDiveEventTicketTypeRepository(DataContext context) : base(context)
        {
        }

        public async Task Create(string title, int capacity)
        {
            await Context.SkyDiveEventTicketTypes.AddAsync(new SkyDiveEventTicketType(title, capacity));
        }

        public void Update(SkyDiveEventTicketType type, string title, int capacity)
        {
            type.Title = title;
            type.Capacity = capacity;
        }
    }
}
