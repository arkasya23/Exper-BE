using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Dtos.Notifications;
using ExperBE.Extensions.ClaimsPrincipal;
using ExperBE.Repositories.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NotificationsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public NotificationsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _repository.Notification.GetAll()
                .AsNoTracking()
                .Where(n => n.UserId == User.GetId())
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            var result = notifications
                .Select(n => new NotificationDto(n));

            return Ok(result);
        }
    }
}
