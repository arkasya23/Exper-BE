using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Extensions.ClaimsPrincipal;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.Extensions.ClaimsPrincipal
{
    [TestClass]
    public class ClaimsPrincipalExtensionsTests
    {
        [TestMethod]
        public void ClaimsPrincipalExtensions_AllMethods_GetsValuesFromClaims()
        {
            var guid = Guid.NewGuid();
            var claimsPrincipal = BuildClaimsPrincipal(guid, "email");
            Assert.AreEqual(guid, claimsPrincipal.GetId());
            Assert.AreEqual("email", claimsPrincipal.GetEmail());
        }

        [TestMethod]
        public void ClaimsPrincipalExtensions_GetId_ReturnsEmptyGuid_IfNoIdClaim()
        {
            var claimsPrincipal = BuildClaimsPrincipal(null, "email");
            Assert.AreEqual(Guid.Empty, claimsPrincipal.GetId());
        }

        [TestMethod]
        public void ClaimsPrincipalExtensions_GetEmail_ReturnsEmptyString_IfNoEmailClaim()
        {
            var claimsPrincipal = BuildClaimsPrincipal(Guid.NewGuid(), null);
            Assert.AreEqual("", claimsPrincipal.GetEmail());
        }

        private System.Security.Claims.ClaimsPrincipal BuildClaimsPrincipal(Guid? id, string? email)
        {
            var claims = new List<Claim>();
            if (id != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, id.ToString()));
            }

            if (email != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
            } 

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new System.Security.Claims.ClaimsPrincipal(identity);

            return claimsPrincipal;
        }
    }
}
