using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SkyDiveTicketing.Core.Entities.User;

namespace SkyDiveTicketing.Application.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public TokenGenerator(IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        public JwToken TokenGeneration(User user, JwtIssuerOptionsModel _jwtOptions, IList<IdentityRole> userRoles)
        {
            var refreshToken = _tokenFactory.GenerateToken();
            if (user.RefreshTokens == null)
                user.RefreshTokens = new List<RefreshToken>();
            user.AddRefreshToken(refreshToken, user.Id.ToString(), _jwtOptions.ExpireTimeTokenInMinute);

            var identity = _jwtFactory.GenerateClaimsIdentity(user.Email, user.Id.ToString());
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
