using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISettingsRepository : IRepository<Settings>
    {
        Task Update(string url, int fileSizeLimitation);

        Task UpdateUserStatusInfo(UserStatus userStatus, string description);
    }
}
