using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Libraries;
using ExperBE.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.Models.Entities
{
    [TestClass]
    public class UserTests
    {
        private static readonly string Email = "email@email.com";
        private static readonly string Password = "testPassword";

        private User _createdUser = null!;

        [TestInitialize]
        public void UserTests_Initialize()
        {
            _createdUser = User.CreateNew(Email, Password);
        }

        [TestMethod]
        public void User_CreateNew_SetsEmail()
        {
            Assert.AreEqual(Email, _createdUser.Email);
        }

        [TestMethod]
        public void User_CreateNew_SetsPassword_And_IsNotPlaintext()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(_createdUser.Password));
            Assert.IsFalse(string.IsNullOrWhiteSpace(_createdUser.PasswordSalt));
            Assert.AreNotEqual(Password, _createdUser.Password);
        }

        [TestMethod]
        public void User_CreateNew_SetPasswordIsVerifiable_ViaValidatePassword()
        {
            Assert.IsTrue(_createdUser.ValidatePassword(Password));
        }

        [TestMethod]
        public void User_ValidatePassword_FailsIfWrongPassword()
        {
            Assert.IsFalse(_createdUser.ValidatePassword("WrongPassword123"));
            Assert.IsFalse(_createdUser.ValidatePassword(""));
        }

        [TestMethod]
        public void User_ValidatePassword_Fails_IfSaltIsChanged()
        {
            _createdUser.PasswordSalt = "newInvalidSalt";
            Assert.IsFalse(_createdUser.ValidatePassword(Password));
        }

        [TestMethod]
        public void User_ValidatePassword_Fails_IfPasswordIsSetByHand()
        {
            _createdUser.Password = Password;
            Assert.IsFalse(_createdUser.ValidatePassword(Password));
        }

        [TestMethod]
        public void User_GenerateJwtToken_GeneratesToken_And_IsValidAtLeastOneWeek()
        {
            var token = _createdUser.GenerateJwtToken();

            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Token);
            Assert.IsTrue(token.ValidUntil > DateTime.UtcNow.AddDays(7));
        }

        [TestMethod]
        public void User_GenerateJwtToken_IsValid()
        {
            var token = _createdUser.GenerateJwtToken();
            var tokenValidationParameters = Jwt.GetTokenValidationParameters();
            new JwtSecurityTokenHandler().ValidateToken(token.Token, tokenValidationParameters, out _);
            // If it did not throw, it is valid
        }
    }
}
