using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExperBE.Controllers;
using ExperBE.Dtos.GroupExpenses;
using ExperBE.Dtos.PersonalExpenses;
using ExperBE.Dtos.Trips;
using ExperBE.Dtos.Users;
using ExperBE.Exceptions;
using ExperBE.Models.Entities;
using ExperBE.Repositories.Wrapper;
using ExperBE.Tests.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;


namespace ExperBE.Tests.Controllers
{
    [TestClass]
    public class GroupExpensesControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private GroupExpensesController _controller;

        private List<Trip> _trips;
        private List<User> _users;
        private List<GroupExpense> _groupExpenses;

        [TestInitialize]
        public void PersonalExpensesControllerTests_Initialize()
        {
            _repository = new Mock<IRepositoryWrapper>();

            _users = new List<User>()
            {
                User.CreateNew("first@email.com", "testPass"),
                User.CreateNew("second@email.com", "testPass")
            };
            _users[0].Id = Guid.NewGuid();
            _users[1].Id = Guid.NewGuid();

            _trips = new List<Trip>()
            {
                new Trip("trip1")
                {
                    Id = Guid.NewGuid(),
                    Users = new List<User>()
                    {
                        _users[0]
                    }
                },
                new Trip("trip2")
                {
                    Id = Guid.NewGuid(),
                    Users = new List<User>()
                    {
                        _users[0],
                        _users[1]
                    }
                }
            };

            _groupExpenses = new List<GroupExpense>
            {
                new GroupExpense("First", 1.0m, false, _users[0].Id, _trips[0].Id)
                {
                    Id = Guid.NewGuid(),
                    Users = new List<GroupExpenseUser>
                    {
                        new GroupExpenseUser(_users[0].Id)
                        {
                            User = _users[0]
                        }
                    },
                    Trip = _trips[0],
                    CreatedBy = _users[0]
                },
                new GroupExpense("Second", 2.0m, false, _users[0].Id, _trips[0].Id)
                {
                    Id = Guid.NewGuid(),
                    Users = new List<GroupExpenseUser>
                    {
                        new GroupExpenseUser(_users[0].Id)
                        {
                            User = _users[0]
                        }
                    },
                    Trip = _trips[0]
                },
                new GroupExpense("Third", 3.0m, false, _users[0].Id, _trips[1].Id)
                {
                    Id = Guid.NewGuid(),
                    Users = new List<GroupExpenseUser>
                    {
                        new GroupExpenseUser(_users[0].Id)
                        {
                            User = _users[0]
                        }
                    },
                    Trip = _trips[1]
                },
                new GroupExpense("First for second user", 1.0m, false, _users[1].Id, _trips[0].Id)
                {
                    Id = Guid.NewGuid(),
                    Users = new List<GroupExpenseUser>
                    {
                        new GroupExpenseUser(_users[0].Id)
                        {
                            User = _users[0]
                        }
                    },
                    Trip = _trips[0]
                }
            };

            _repository.Setup(r => r.Trip.GetAll()).Returns(_trips.AsQueryable().BuildMock().Object);
            _repository.Setup(r => r.User.GetAll()).Returns(_users.AsQueryable().BuildMock().Object);
            _repository.Setup(r => r.GroupExpense.GetAll())
                .Returns(_groupExpenses.AsQueryable().BuildMock().Object);
            _repository.Setup(r => r.Notification.Add(It.IsAny<Notification>()));
            _repository.Setup(r => r.GroupExpenseUser.Remove(It.IsAny<GroupExpenseUser>()));
                _controller = new GroupExpensesController(_repository.Object);

            // Setup claims
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, _users.First().Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _users.First().Email)
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(claimsPrincipal);

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = mockContext.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task GroupExpensesController_CreateGroupExpense_ValidatesDto_AndThrowsIfInvalid()
        {
            var dto = new GroupExpenseCreateDto();
            await _controller.CreateGroupExpense(dto);
        }

        [TestMethod]
        public async Task GroupExpensesController_CreateGroupExpense_ReturnsNotFound_IfInvalidTrip()
        {
            var dto = new GroupExpenseCreateDto
            {
                Amount = 1.0m,
                Description = "Random",
                DivideBetweenAllMembers = true,
                TripId = Guid.NewGuid()
            };
            var res = await _controller.CreateGroupExpense(dto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task GroupExpensesController_CreateGroupExpense_ReturnsNotFound_IfUserNotInTrip()
        {
            var dto = new GroupExpenseCreateDto
            {
                Amount = 1.0m,
                Description = "Random",
                DivideBetweenAllMembers = true,
                TripId = _trips.First().Id
            };
            _trips.First().Users.Clear();
            var res = await _controller.CreateGroupExpense(dto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task GroupExpensesController_CreateGroupExpense_AddsGroupExpense()
        {
            var dto = new GroupExpenseCreateDto
            {
                Amount = 1.0m,
                Description = "Random",
                DivideBetweenAllMembers = true,
                TripId = _trips.First().Id
            };
            var res = await _controller.CreateGroupExpense(dto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            _repository.Verify(r => r.GroupExpense.Add(
                It.Is<GroupExpense>(ge => 
                    ge.Amount == dto.Amount && 
                    ge.Description == dto.Description &&
                    ge.DivideBetweenAllMembers == dto.DivideBetweenAllMembers &&
                    ge.TripId == dto.TripId)), Times.Once);
        }

        [TestMethod]
        public async Task GroupExpensesController_CreateGroupExpense_ReturnsDto()
        {
            var createDto = new GroupExpenseCreateDto
            {
                Amount = 1.0m,
                Description = "Random",
                DivideBetweenAllMembers = true,
                TripId = _trips.First().Id
            };
            var res = await _controller.CreateGroupExpense(createDto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = res.Value as GroupExpenseDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(createDto.Amount, dto.Amount);
            Assert.AreEqual(createDto.Description, dto.Description);
            Assert.AreEqual(createDto.DivideBetweenAllMembers, dto.DivideBetweenAllMembers);
            Assert.AreEqual(createDto.TripId, dto.TripId);
        }

        [TestMethod]
        public async Task GroupExpensesController_CreateGroupExpense_AddsNotificationsToEveryone_IfDivideBetweenAllMembers()
        {
            var createDto = new GroupExpenseCreateDto
            {
                Amount = 1.0m,
                Description = "Random",
                DivideBetweenAllMembers = true,
                TripId = _trips.First().Id
            };
            _trips.First().Users.Add(_users[1]);
            var res = await _controller.CreateGroupExpense(createDto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            _repository.Verify(r => r.Notification.Add(It.IsAny<Notification>()), Times.Once);
        }

        [TestMethod]
        public async Task GroupExpensesController_CreateGroupExpense_AddsNotificationsToMembers_IfNotDivideBetweenAllMembers()
        {
            var createDto = new GroupExpenseCreateDto
            {
                Amount = 1.0m,
                Description = "Random",
                DivideBetweenAllMembers = false,
                TripId = _trips.First().Id,
                UserIds = new List<Guid>()
                {
                    _users[1].Id
                }
            };
            _trips.First().Users.Add(_users[1]);
            var res = await _controller.CreateGroupExpense(createDto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            _repository.Verify(r => r.Notification.Add(It.IsAny<Notification>()), Times.Once);
        }

        [TestMethod]
        public async Task GroupExpensesController_GetAllGroupExpensesByTripId_ReturnsAsExpected()
        {
            Guid tripId = _trips[0].Id;
            var res = await _controller.GetAllGroupExpensesByTripId(tripId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = (res.Value as IEnumerable<GroupExpenseDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(3, dto.Count);
            Assert.IsTrue(dto.All(ge => _groupExpenses.Any(x => x.Id == ge.Id)));
        }

        [TestMethod]
        public async Task GroupExpensesController_GetAllGroupExpensesByTripId_ReturnsEmptyList_IfInvalidTripId()
        {
            Guid tripId = Guid.NewGuid();
            var res = await _controller.GetAllGroupExpensesByTripId(tripId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = (res.Value as IEnumerable<GroupExpenseDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task GroupExpensesController_UpdateGroupExpense_ValidatesDtoAndThrowsIfInvalid()
        {
            var updateDto = new GroupExpenseUpdateDto();
            var groupExpenseId = _groupExpenses[0].Id;
            await _controller.UpdateGroupExpense(updateDto, groupExpenseId);
        }

        [TestMethod]
        public async Task GroupExpensesController_UpdateGroupExpense_ReturnsNotFound_IfInvalidId()
        {
            var updateDto = new GroupExpenseUpdateDto
            {
                Amount = 99.9m,
                Description = "Yes!",
                DivideBetweenAllMembers = true
            };
            var groupExpenseId = Guid.NewGuid();
            var res = await _controller.UpdateGroupExpense(updateDto, groupExpenseId) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task GroupExpensesController_UpdateGroupExpense_ReturnsUpdatedDto()
        {
            var updateDto = new GroupExpenseUpdateDto
            {
                Amount = 99.9m,
                Description = "Yes!",
                DivideBetweenAllMembers = true
            };
            var groupExpenseId = _groupExpenses[0].Id;
            var res = await _controller.UpdateGroupExpense(updateDto, groupExpenseId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = res.Value as GroupExpenseDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(updateDto.Amount, dto.Amount);
            Assert.AreEqual(updateDto.Description, dto.Description);
            Assert.AreEqual(updateDto.DivideBetweenAllMembers, dto.DivideBetweenAllMembers);
        }

        [TestMethod]
        public async Task GroupExpensesController_UpdateGroupExpense_SendsNotificationsToEveryoneButCreatingUser_IfDivideBetweenAllMembers()
        {
            var updateDto = new GroupExpenseUpdateDto
            {
                Amount = 99.9m,
                Description = "Yes!",
                DivideBetweenAllMembers = true
            };
            var groupExpenseId = _groupExpenses[0].Id;
            _groupExpenses[0].Trip.Users.Add(_users[1]);

            var res = await _controller.UpdateGroupExpense(updateDto, groupExpenseId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            _repository.Verify(r => r.Notification.Add(
                It.Is<Notification>(n => _groupExpenses[0].Trip.Users.Any(u => u.Id == n.UserId))));
        }

        [TestMethod]
        public async Task GroupExpensesController_UpdateGroupExpense_SendsNotificationsToSpecifiedUsersButCreatingUser_IfDivideBetweenAllMembers()
        {
            var updateDto = new GroupExpenseUpdateDto
            {
                Amount = 99.9m,
                Description = "Yes!",
                DivideBetweenAllMembers = false,
                UserIds = new List<Guid>{_users[1].Id}
            };
            var groupExpenseId = _groupExpenses[0].Id;

            var res = await _controller.UpdateGroupExpense(updateDto, groupExpenseId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            _repository.Verify(r => r.Notification.Add(
                It.Is<Notification>(n => n.UserId == _users[1].Id)));
        }
    }
}
