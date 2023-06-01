using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        public SettingsRepository(DataContext context) : base(context)
        {
        }

        public async Task UpdateUrl(string url)
        {
            var settings = Context.Settings.FirstOrDefault();

            if (settings is null)
                await Context.Settings.AddAsync(new Settings() { TermsAndConditionsUrl = url });
            else
                settings.TermsAndConditionsUrl = url;
        }

        public async Task UpdateUserStatusInfo(UserStatus userStatus, string description)
        {
            var settings = Context.Settings.FirstOrDefault();

            if (settings is null)
                await Context.Settings.AddAsync(new Settings()
                {
                    UserStatusInfo = new List<UserStatusInfo>() { new UserStatusInfo(userStatus, description) }
                });
            else
                settings.SetStatus(userStatus, description);
        }
    }
}
