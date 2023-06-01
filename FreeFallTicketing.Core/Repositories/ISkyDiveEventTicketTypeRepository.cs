using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISkyDiveEventTicketTypeRepository : IRepository<SkyDiveEventTicketType>
    {
        Task Create(string title, int capacity);
        void Update(SkyDiveEventTicketType type, string title, int capacity);
    }
}
