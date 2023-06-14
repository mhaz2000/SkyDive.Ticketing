using Microsoft.AspNetCore.Identity;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IRoleRepository : IRepository<IdentityRole<Guid>>
    {
        IQueryable<IdentityRole<Guid>> GetUserRoles(User user);
    }
}
