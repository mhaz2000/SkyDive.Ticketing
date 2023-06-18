using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISkyDiveEventStatusRepository : IRepository<SkyDiveEventStatus>
    {
        Task Create(string title, bool reservable);
        void Update(string title, bool reservable, SkyDiveEventStatus status);
    }
}
