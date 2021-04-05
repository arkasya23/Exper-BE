using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExperBE.Controllers;
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
    public class PersonalExpensesControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private PersonalExpensesController _controller;

        private List<Trip> _trips;
        private List<User> _users;
        private List<PersonalExpense> _personalExpenses;

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

            _personalExpenses = new List<PersonalExpense>
            {
                new PersonalExpense("First", 1.0m, _users[0].Id, _trips[0].Id)
                {
                    Id = Guid.NewGuid()
                },
                new PersonalExpense("Second", 2.0m, _users[0].Id, _trips[0].Id)
                {
                    Id = Guid.NewGuid()
                },
                new PersonalExpense("Third", 3.0m, _users[0].Id, _trips[1].Id)
                {
                    Id = Guid.NewGuid()
                },
                new PersonalExpense("First for second user", 1.0m, _users[1].Id, _trips[0].Id)
                {
                    Id = Guid.NewGuid()
                }
            };

            _repository.Setup(r => r.Trip.GetAll()).Returns(_trips.AsQueryable().BuildMock().Object);
            _repository.Setup(r => r.User.GetAll()).Returns(_users.AsQueryable().BuildMock().Object);
            _repository.Setup(r => r.PersonalExpense.GetAll())
                .Returns(_personalExpenses.AsQueryable().BuildMock().Object);
            _controller = new PersonalExpensesController(_repository.Object);

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
        public async Task PersonalExpensesController_CreatePersonalExpense_ValidatesDto_AndThrowsIfInvalid()
        {
            var dto = new PersonalExpenseCreateDto();
            await _controller.CreatePersonalExpense(dto);
        }

        [TestMethod]
        public async Task PersonalExpensesController_CreatePersonalExpense_ReturnsNotFound_IfInvalidTrip()
        {
            var createDto = new PersonalExpenseCreateDto
            {
                Amount = 5.0m,
                Description = "Desc",
                TripId = Guid.NewGuid()
            };
            var res = await _controller.CreatePersonalExpense(createDto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task PersonalExpensesController_CreatePersonalExpense_ReturnsNotFound_IfUserNotInTrip()
        {
            var createDto = new PersonalExpenseCreateDto
            {
                Amount = 5.0m,
                Description = "Desc",
                TripId = _trips[0].Id
            };
            _trips[0].Users.Clear();
            var res = await _controller.CreatePersonalExpense(createDto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task PersonalExpensesController_CreatePersonalExpense_CreatesExpense_WithGivenValues()
        {
            var createDto = new PersonalExpenseCreateDto
            {
                Amount = 5.0m,
                Description = "Desc",
                TripId = _trips[0].Id
            };
            var res = await _controller.CreatePersonalExpense(createDto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            _repository.Verify(r => r.PersonalExpense.Add(It.Is<PersonalExpense>(e => 
                e.Amount == createDto.Amount &&
                e.TripId == createDto.TripId &&
                e.Description == createDto.Description &&
                e.CreatedById == _users[0].Id)));
        }

        [TestMethod]
        public async Task PersonalExpensesController_CreatePersonalExpense_ReturnsDto()
        {
            var createDto = new PersonalExpenseCreateDto
            {
                Amount = 5.0m,
                Description = "Desc",
                TripId = _trips[0].Id
            };
            var res = await _controller.CreatePersonalExpense(createDto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = res.Value as PersonalExpenseDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(createDto.Amount, dto.Amount);
            Assert.AreEqual(createDto.Description, dto.Description);
            Assert.AreEqual(createDto.TripId, dto.TripId);
        }

        [TestMethod]
        public async Task PersonalExpensesController_GetAllPersonalExpensesByTripId_ReturnsEmptyList_IfInvalidTrip()
        {
            var tripId = Guid.NewGuid();
            var res = await _controller.GetAllPersonalExpensesByTripId(tripId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = (res.Value as IEnumerable<PersonalExpenseDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Count);
        }

        [TestMethod]
        public async Task PersonalExpensesController_GetAllPersonalExpensesByTripId_ReturnsEmptyList_IfNoExpenses()
        {
            var tripId = _trips[1].Id;
            _personalExpenses.RemoveAll(e => e.TripId == tripId);
            var res = await _controller.GetAllPersonalExpensesByTripId(tripId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = (res.Value as IEnumerable<PersonalExpenseDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Count);
        }

        [TestMethod]
        public async Task PersonalExpensesController_GetAllPersonalExpensesByTripId_ReturnsTrips()
        {
            var tripId = _trips[0].Id;
            var existingExpenses = _personalExpenses
                .Where(e => e.TripId == tripId && e.CreatedById == _users[0].Id)
                .ToList();
            var res = await _controller.GetAllPersonalExpensesByTripId(tripId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = (res.Value as IEnumerable<PersonalExpenseDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(existingExpenses.Count, dto.Count);
            Assert.IsTrue(dto.All(x => existingExpenses.Any(e => 
                e.Id == x.Id &&
                e.Amount == x.Amount &&
                e.Description == x.Description)));
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task PersonalExpensesController_UpdatePersonalExpense_ValidatesDto_AndThrowsIfInvalid()
        {
            var expenseId = _personalExpenses[0].Id;
            var updateDto = new PersonalExpenseUpdateDto();
            await _controller.UpdatePersonalExpense(updateDto, expenseId);
        }

        [TestMethod]
        public async Task PersonalExpensesController_UpdatePersonalExpense_ReturnsNotFound_IfInvalidId()
        {
            var expenseId = Guid.NewGuid();
            var updateDto = new PersonalExpenseUpdateDto
            {
                Amount = 123.12m,
                Description = "New Description"
            };

            var res = await _controller.UpdatePersonalExpense(updateDto, expenseId) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task PersonalExpensesController_UpdatePersonalExpense_ReturnsNotFound_IfNotCurrentUsersExpense()
        {
            var expenseId = _personalExpenses[0].Id;
            var updateDto = new PersonalExpenseUpdateDto
            {
                Amount = 123.12m,
                Description = "New Description"
            };
            _personalExpenses[0].CreatedById = Guid.NewGuid();

            var res = await _controller.UpdatePersonalExpense(updateDto, expenseId) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task PersonalExpensesController_UpdatePersonalExpense_ReturnsUpdatedDto()
        {
            var expenseId = _personalExpenses[0].Id;
            var updateDto = new PersonalExpenseUpdateDto
            {
                Amount = 123.12m,
                Description = "New Description"
            };

            var res = await _controller.UpdatePersonalExpense(updateDto, expenseId) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsTrue(res.IsSuccessStatusCode());
            var dto = res.Value as PersonalExpenseDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(updateDto.Amount, dto.Amount);
            Assert.AreEqual(updateDto.Description, dto.Description);
        }
    }
}
