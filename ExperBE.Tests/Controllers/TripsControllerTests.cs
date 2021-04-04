using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExperBE.Controllers;
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
    public class TripsControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private TripsController _controller;

        private List<Trip> _trips;
        private List<User> _users;

        [TestInitialize]
        public void TripsControllerTests_Initialize()
        {
            _repository = new Mock<IRepositoryWrapper>();

            _users = new List<User>()
            {
                User.CreateNew("testEmail@email.email", "testPass"),
                User.CreateNew("second@email.com", "testPass")
            };
            _users[0].Id = Guid.NewGuid();
            _users[1].Id = Guid.NewGuid();

            _trips = new List<Trip>()
            {
                new Trip("trip1")
                {
                    Users = new List<User>()
                    {
                        _users[0]
                    }
                },
                new Trip("trip2")
                {
                    Users = new List<User>()
                    {
                        _users[0],
                        _users[1]
                    }
                }
            };

            _repository.Setup(r => r.Trip.GetAll()).Returns(_trips.AsQueryable().BuildMock().Object);
            _repository.Setup(r => r.User.GetAll()).Returns(_users.AsQueryable().BuildMock().Object);
            _controller = new TripsController(_repository.Object);

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
        public async Task TripsController_CreateTrip_ValidatesDto_AndThrowsIfNotValid()
        {
            var dto = new TripCreateDto()
            {
                Name = null!
            };

            await _controller.CreateTrip(dto);
        }

        [TestMethod]
        public async Task TripsController_CreateTrip_CreatesTrip()
        {
            var dto = new TripCreateDto()
            {
                Name = "testTrip"
            };

            await _controller.CreateTrip(dto);
            _repository.Verify(r => r.Trip.Add(It.Is<Trip>(t => t.Name == dto.Name)), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task TripsController_CreateTrip_AddsRequestingUser_IfNotAlreadyInUsersList()
        {
            var userId = _users[1].Id;
            var dto = new TripCreateDto()
            {
                Name = "testTrip",
                UserIds = new List<Guid>() {userId}
            };

            await _controller.CreateTrip(dto);
            _repository.Verify(r => r.Trip.Add(It.Is<Trip>(t => t.Users.Any(u => u.Id == _users.First().Id))), Times.AtLeastOnce);
            _repository.Verify(r => r.Trip.Add(It.Is<Trip>(t => t.Users.Any(u => u.Id == userId))), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task TripsController_CreateTrip_ReturnsDto()
        {
            var dto = new TripCreateDto()
            {
                Name = "testTrip"
            };

            var res = await _controller.CreateTrip(dto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsInstanceOfType(res.Value, typeof(TripDto));
        }

        [TestMethod]
        public async Task TripsController_GetAllTrips_ReturnsAllTrips()
        {
            var res = await _controller.GetAllTrips() as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsInstanceOfType(res.Value, typeof(IEnumerable<TripDto>));
            var dto = res.Value as IEnumerable<TripDto>;
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto.Any(t => t.Name == _trips[0].Name));
            Assert.IsTrue(dto.Any(t => t.Name == _trips[1].Name));
        }

        [TestMethod]
        public async Task TripsController_GetAllTrips_ReturnsAllTrips_WithoutTripsNotContainingUser()
        {
            _trips[1].Users = new List<User>();
            var res = await _controller.GetAllTrips() as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsInstanceOfType(res.Value, typeof(IEnumerable<TripDto>));
            var dto = res.Value as IEnumerable<TripDto>;
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto.Any(t => t.Name == _trips[0].Name));
            Assert.IsFalse(dto.Any(t => t.Name == _trips[1].Name));
        }

        [TestMethod]
        public async Task TripsController_GetById_ReturnsNotFound_IfBadId()
        {
            _trips[0].Id = Guid.NewGuid();
            var res = await _controller.GetById(Guid.NewGuid()) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task TripsController_GetById_ReturnsNotFound_IfUserNotInTrip()
        {
            _trips[0].Users = new List<User>();
            _trips[0].Id = Guid.NewGuid();
            var res = await _controller.GetById(_trips[0].Id) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task TripsController_GetById_ReturnsTripDto()
        {
            _trips[0].Id = Guid.NewGuid();
            var res = await _controller.GetById(_trips[0].Id) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsInstanceOfType(res.Value, typeof(TripDto));
            var dto = res.Value as TripDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(_trips[0].Name, dto.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task TripsController_AddUsersToTrip_ValidatesDtoAndThrowsIfInvalid()
        {
            _trips[0].Id = Guid.NewGuid();
            var dto = new TripAddUsersDto()
            {
                TripId = _trips[0].Id
            };
            await _controller.AddUsersToTrip(dto);
        }

        [TestMethod]
        public async Task TripsController_AddUsersToTrip_ReturnsNotFound_IfTripNotFound()
        {
            _trips[0].Id = Guid.NewGuid();
            var dto = new TripAddUsersDto()
            {
                TripId = Guid.NewGuid(),
                UserIds = new List<Guid>()
                {
                    _users[1].Id
                }
            };
            var res = await _controller.AddUsersToTrip(dto) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.IsSuccessStatusCode());
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task TripsController_AddUsersToTrip_AddsUserToTrip()
        {
            _trips[0].Id = Guid.NewGuid();
            var addDto = new TripAddUsersDto()
            {
                TripId = _trips[0].Id,
                UserIds = new List<Guid>()
                {
                    _users[1].Id
                }
            };
            var res = await _controller.AddUsersToTrip(addDto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsInstanceOfType(res.Value, typeof(TripDto));
            var dto = res.Value as TripDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(_trips[0].Name, dto.Name);
            Assert.IsTrue(dto.Users.Any(u => u.Id == _users[1].Id));
        }

        [TestMethod]
        public async Task TripsController_AddUsersToTrip_DoesNotAddExistingUserToTrip()
        {
            _trips[0].Id = Guid.NewGuid();
            var addDto = new TripAddUsersDto()
            {
                TripId = _trips[0].Id,
                UserIds = new List<Guid>()
                {
                    _users[0].Id
                }
            };
            var res = await _controller.AddUsersToTrip(addDto) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsInstanceOfType(res.Value, typeof(TripDto));
            var dto = res.Value as TripDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(_trips[0].Name, dto.Name);
            Assert.IsTrue(dto.Users.Single(u => u.Id == _users[0].Id) != null);
        }
    }
}
