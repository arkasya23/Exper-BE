using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Libraries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.Libraries
{
    [TestClass]
    public class PasswordHashTests
    {
        [TestMethod]
        public void PasswordHash_Encrypt_GeneratesBothHashAndSalt()
        {
            (string hash, string salt) = PasswordHash.Encrypt("pass");
            Assert.IsFalse(string.IsNullOrWhiteSpace(hash));
            Assert.IsFalse(string.IsNullOrWhiteSpace(salt));
        }

        [TestMethod]
        public void PasswordHash_Verify_CanVerifyEncryptedPassword()
        {
            var password = "TestPass";
            (string hash, string salt) = PasswordHash.Encrypt(password);
            
            Assert.IsTrue(PasswordHash.Verify(password, hash, salt));
        }

        [TestMethod]
        public void PasswordHash_Verify_IfWrongParameters_ReturnsFalse()
        {
            var password = "TestPass";
            (string hash, string salt) = PasswordHash.Encrypt(password);

            Assert.IsFalse(PasswordHash.Verify("wrongPass", hash, salt));
            Assert.IsFalse(PasswordHash.Verify("", hash, salt));
            Assert.IsFalse(PasswordHash.Verify(password, "wrongHash", salt));
            Assert.IsFalse(PasswordHash.Verify(password, "", salt));
            Assert.IsFalse(PasswordHash.Verify(password, hash, "wrongSalt"));
            Assert.IsFalse(PasswordHash.Verify(password, hash, ""));
        }
    }
}
