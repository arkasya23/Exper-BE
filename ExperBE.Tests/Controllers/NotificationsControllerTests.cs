using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExperBE.Controllers;
using ExperBE.Dtos.Notifications;
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
    public class NotificationsControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private NotificationsController _controller;

        private List<User> _users;
        private List<Notification> _notifications;

        [TestInitialize]
        public void NotificationsControllerTests_Initialize()
        {
            _users = new List<User>()
            {
                User.CreateNew("testEmail@email.email", "testPass"),
                User.CreateNew("second@email.com", "testPass")
            };
            _users[0].Id = Guid.NewGuid();
            _users[1].Id = Guid.NewGuid();

            _notifications = new List<Notification>
            {
                new Notification("Notification 0.0", "Description 0.0", _users[0].Id),
                new Notification("Notification 0.1", "Description 0.1", _users[0].Id),
                new Notification("Notification 1.0", "Description 1.0", _users[1].Id)
            };

            _repository = new Mock<IRepositoryWrapper>();
            _repository.Setup(r => r.Notification.GetAll()).Returns(_notifications.AsQueryable().BuildMock().Object);
            _controller = new NotificationsController(_repository.Object);

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
        public async Task NotificationsController_GetAllNotifications_ReturnsSuccess()
        {
            var res = await _controller.GetAllNotifications() as IStatusCodeActionResult;
            Assert.IsTrue(res.IsSuccessStatusCode());
        }

        [TestMethod]
        public async Task NotificationsController_GetAllNotifications_ReturnsOnlyCurrentUsersNotifications()
        {
            var notificationsForCurrentUser = _notifications
                .Where(n => n.UserId == _users[0].Id)
                .ToList();

            var res = await _controller.GetAllNotifications() as OkObjectResult;
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            var dto = (res.Value as IEnumerable <NotificationDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(2, dto.Count);
            Assert.IsTrue(dto.All(n => notificationsForCurrentUser.Any(x => x.Title == n.Title)));
        }

        [TestMethod]
        public async Task NotificationsController_GetAllNotifications_ReturnsEmptyList_IfNoNotifications()
        {
            _notifications.RemoveAll(n => n.UserId == _users[0].Id);

            var res = await _controller.GetAllNotifications() as OkObjectResult;
            Assert.IsTrue(res.IsSuccessStatusCode());
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Value);
            var dto = (res.Value as IEnumerable<NotificationDto>)?.ToList();
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Count);
        }


    }
}
