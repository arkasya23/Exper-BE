using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Dtos.Trips;
using ExperBE.Dtos.Users;
using ExperBE.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.Exceptions
{
    [TestClass]
    public class BadRequestExceptionTests
    {
        [TestMethod]
        public void BadRequestException_Constructor_AsExpected()
        {
            var ex = new BadRequestException("property", "message");
            Assert.AreEqual(ex.Error.Errors.First().Key, "property");
            Assert.AreEqual(ex.Error.Errors.First().Value.First(), "message");
        }

        [TestMethod]
        public async Task BadRequestException_ValidationResultConstructor_AsExpected()
        {
            var userLoginDto = new UserLoginDto();
            var validationResult = await userLoginDto.Validate();
            var exception = new BadRequestException(validationResult);
            Assert.IsTrue(exception.Error.Errors.Any(err => err.Key == nameof(userLoginDto.Email)));
            Assert.IsTrue(exception.Error.Errors.Any(err => err.Key == nameof(userLoginDto.Password)));
        }
    }
}
