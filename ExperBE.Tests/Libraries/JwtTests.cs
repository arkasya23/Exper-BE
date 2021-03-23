using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Libraries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.Libraries
{
    [TestClass]
    public class JwtTests
    {
        [TestMethod]
        public void Jwt_GetTokenValidationParameters_ReturnsTokenValidationParameters()
        {
            var parameters = Jwt.GetTokenValidationParameters();
            Assert.IsNotNull(parameters);
        }

        [TestMethod]
        public void Jwt_GetTokenValidationParameters_SigningKeyAndLifetimeAreValidated()
        {
            var parameters = Jwt.GetTokenValidationParameters();
            Assert.IsTrue(parameters.ValidateIssuerSigningKey);
            Assert.IsTrue(parameters.ValidateLifetime);
        }

        [TestMethod]
        public void Jwt_GetTokenValidationParameters_DoesNotValidateOthers()
        {
            var parameters = Jwt.GetTokenValidationParameters();
            Assert.IsFalse(parameters.ValidateActor);
            Assert.IsFalse(parameters.ValidateAudience);
            Assert.IsFalse(parameters.ValidateIssuer);
            Assert.IsFalse(parameters.ValidateTokenReplay);
        }

        [TestMethod]
        public void Jwt_GenerateJwtToken_GeneratesAJwtToken_WithCorrectClaims()
        {
            var id = "id";
            var email = "email";
            var jwtToken = Jwt.GenerateJwtToken(id, email);
            Assert.IsTrue(jwtToken.Claims.Any(c => c.Value == id && c.Type == JwtRegisteredClaimNames.Sub));
            Assert.IsTrue(jwtToken.Claims.Any(c => c.Value == email && c.Type == JwtRegisteredClaimNames.Email));
            Assert.IsTrue(jwtToken.Claims.Any(c => !string.IsNullOrWhiteSpace(c.Value) && c.Type == JwtRegisteredClaimNames.Jti));
        }

        [TestMethod]
        public void Jwt_GenerateJwtToken_GeneratesToken_ExpiringIn14Days()
        {
            var jwtToken = Jwt.GenerateJwtToken("", "");
            Assert.IsTrue(jwtToken.ValidTo > DateTime.UtcNow.AddDays(13).AddHours(23).AddMinutes(59));
            Assert.IsTrue(jwtToken.ValidFrom > DateTime.MinValue);
        }
    }
}
