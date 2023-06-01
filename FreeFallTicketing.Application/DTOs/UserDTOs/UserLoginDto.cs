using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserLoginDto
    {
        public string TokenType { get; internal set; }
        public string AuthToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public int ExpiresIn { get; internal set; }
    }
}
