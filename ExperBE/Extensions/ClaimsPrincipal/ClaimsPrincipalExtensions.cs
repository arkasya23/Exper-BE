using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ExperBE.Extensions.ClaimsPrincipal
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
        {
            var value = claimsPrincipal.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            return value == null 
                ? Guid.Empty 
                : Guid.Parse(value);
        }

        public static string GetEmail(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
        {
            var value = claimsPrincipal.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            return value ?? "";
        }
    }
}
