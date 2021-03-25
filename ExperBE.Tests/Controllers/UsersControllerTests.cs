using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Controllers;
using ExperBE.Dtos.User;
using ExperBE.Exceptions;
using ExperBE.Models.Entities;
using ExperBE.Repositories.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;

namespace ExperBE.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private UsersController _controller;

        [TestInitialize]
        public void UsersControllerTests_Initialize()
        {
            _repository = new Mock<IRepositoryWrapper>();

            var users = new List<User>()
            {
                User.CreateNew("test@test.com", "password"),
                User.CreateNew("test2@test.com", "password2")
            };

            _repository.Setup(r => r.User.GetAll()).Returns(users.AsQueryable().BuildMock().Object);
            _controller = new UsersController(_repository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task UsersController_Register_ValidatesDto_AndThrowsIfInvalid()
        {
            var dto = new UserRegisterDto()
            {
                Email = "",
                Password = ""
            };

            await _controller.Register(dto);
        }

        [TestMethod]
        public async Task UsersController_Register_IfInvalidDto_ExceptionContainsInvalidFields()
        {
            var dto = new UserRegisterDto()
            {
                Email = "",
                Password = ""
            };

            try
            {
                await _controller.Register(dto);
            }
            catch (BadRequestException ex)
            {
                Assert.IsTrue(ex.Error.Errors.Any(error => error.Key.Equals("Email", StringComparison.OrdinalIgnoreCase)));
                Assert.IsTrue(ex.Error.Errors.Any(error => error.Key.Equals("Password", StringComparison.OrdinalIgnoreCase)));
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task UsersController_Register_IfUserWithSameEmailExists_ThrowsException()
        {
            var dto = new UserRegisterDto()
            {
                Email = "test@test.com",
                Password = "anotherPass"
            };

            await _controller.Register(dto);
        }

        [TestMethod]
        public async Task UsersController_Register_AddsToRepo_AndSaves()
        {
            var dto = new UserRegisterDto()
            {
                Email = "newEmail@test.com",
                Password = "superStrongPass!123"
            };

            var result = (ObjectResult) await _controller.Register(dto);

            Assert.AreEqual(200, result.StatusCode);
            _repository.Verify(r => r.User.Add(It.Is<User>(u => u.Email == dto.Email)), Times.Once);
            _repository.Verify(r => r.SaveAsync(), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task UsersController_Register_ReturnsToken()
        {
            var dto = new UserRegisterDto()
            {
                Email = "newEmail@test.com",
                Password = "superStrongPass!123"
            };

            var result = (ObjectResult)await _controller.Register(dto);

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(typeof(UserTokenDto), result.Value.GetType());
            Assert.IsFalse(string.IsNullOrWhiteSpace(((UserTokenDto)result.Value).Token));
        }
    }
}
