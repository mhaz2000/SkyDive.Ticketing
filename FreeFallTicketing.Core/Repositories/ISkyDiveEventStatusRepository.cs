using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISkyDiveEventStatusRepository : IRepository<SkyDiveEventStatus>
    {
        Task Create(string title);
        void Update(string title, SkyDiveEventStatus status);
    }
}
