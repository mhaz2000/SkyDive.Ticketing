using SkyDiveTicketing.Core.Entities;
using System.Security.Claims;

namespace SkyDiveTicketing.Application.Helpers
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(User user, IList<string> userRoles, IEnumerable<string> roleIds, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
