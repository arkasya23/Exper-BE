using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExperBE.Models.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExperBE.Libraries
{
    public static class Jwt
    {
        public static TokenValidationParameters GetTokenValidationParameters()
        {
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ExperConfiguration.JwtSecret));
            return new TokenValidationParameters()
            {
                IssuerSigningKey = signingKey,
                ValidateActor = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateTokenReplay = false
            };
        }

        /// <summary>
        /// Generates a new JWT for a given user
        /// </summary>
        public static JwtSecurityToken GenerateJwtToken(string id, string email)
        {
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ExperConfiguration.JwtSecret));
            var claims = new Claim[3];
            claims[0] = new Claim(JwtRegisteredClaimNames.Sub, id);
            claims[1] = new Claim(JwtRegisteredClaimNames.Email, email);
            claims[2] = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());

            var expires = DateTime.UtcNow.Add(new TimeSpan(14, 0, 0, 0));
            var token = new JwtSecurityToken(
                issuer: "fmi",
                audience: "fmi",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
