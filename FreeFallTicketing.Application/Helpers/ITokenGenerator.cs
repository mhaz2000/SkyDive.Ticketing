using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace SkyDiveTicketing.Application.Helpers
{
    public interface ITokenGenerator
    {
        JwToken TokenGeneration(User user, JwtIssuerOptionsModel jwtOptions, IList<IdentityRole<Guid>> userRoles);
    }
}
