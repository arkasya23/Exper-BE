using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Libraries;

namespace ExperBE.Tests.TestUtils
{
    public static class TestUtilMethods
    {
        public static ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenValidationParameters = Jwt.GetTokenValidationParameters();
            return new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out _);
        }
    }
}
