using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Extensions.IQueryable;
using ExperBE.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.Extensions.IQueryable
{
    [TestClass]
    public class UserExtensionsTests
    {
        private List<User> Users = null!;

        [TestInitialize]
        public void UserExtensionsTests_Initialize()
        {
            Users = new List<User>
            {
                User.CreateNew("test@test.com", "testPassword"), 
                User.CreateNew("test2@test.com", "testPassword"),
                User.CreateNew("test3@test.com", "testPassword")
            };
        }

        [TestMethod]
        public void UserExtensions_ForEmail_ReturnsAsExpected()
        {
            var email = Users.First().Email;
            var filteredUsers = Users.AsQueryable().ForEmail(email).ToList();
            Assert.IsTrue(filteredUsers.All(u => u.Email == email));

            email = Users.ElementAt(2).Email;
            filteredUsers = Users.AsQueryable().ForEmail(email).ToList();
            Assert.IsTrue(filteredUsers.All(u => u.Email == email));

            email = "randomemailthatcertainlydoesntexist@email.com";
            filteredUsers = Users.AsQueryable().ForEmail(email).ToList();
            Assert.IsTrue(filteredUsers.Count == 0);
        }
    }
}
