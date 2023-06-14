using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static SkyDiveTicketing.Core.Entities.User;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IUnitOfWork _unitOfWork;

        public TokenGenerator(IJwtFactory jwtFactory, ITokenFactory tokenFactory, IUnitOfWork unitOfWork)
        {
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _unitOfWork = unitOfWork;
        }

        public JwToken TokenGeneration(User user, JwtIssuerOptionsModel _jwtOptions, IList<IdentityRole<Guid>> userRoles)
        {
            var refreshToken = _tokenFactory.GenerateToken();

            var identity = _jwtFactory.GenerateClaimsIdentity(user.PhoneNumber, user.Id.ToString());
            if (identity == null)
            {
                throw new SystemException("در فراخوانی و تطابق اطلاعات حساب کاربری خطایی رخ داده است!");
            }

            var userRoleNames = userRoles != null ? userRoles.Select(c => c.Name).ToList() : null;
            var userRoleIds = userRoles != null ? userRoles.Select(c => c.Id.ToString()).ToList() : null;

            var generatedToken = GenerateJwt(user, userRoleNames, userRoleIds, identity, _jwtFactory,
                refreshToken, _jwtOptions.ExpireTimeTokenInMinute);

            return generatedToken;
        }

        public static JwToken GenerateJwt(User user, IList<string> userRoles, IReadOnlyCollection<string> userRoleIds, ClaimsIdentity identity,
            IJwtFactory jwtFactory, string refreshToken, int refreshTime)
        {
            var result = new JwToken
            {
                TokenType = "Bearer",
                AuthToken = jwtFactory.GenerateEncodedToken(user, userRoles, userRoleIds, identity),
                RefreshToken = refreshToken,
                expires_in = refreshTime,
            };

            return result;
        }

    }
}
