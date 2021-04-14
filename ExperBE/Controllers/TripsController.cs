using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Dtos.Trips;
using ExperBE.Exceptions;
using ExperBE.Extensions.ClaimsPrincipal;
using ExperBE.Extensions.FluentValidation;
using ExperBE.Models.Entities;
using ExperBE.Repositories.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TripsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public TripsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTrip(TripCreateDto dto)
        {
            await dto.Validate().ThrowIfInvalid();
            var userIds = dto.UserIds?.ToList() ?? new List<Guid>();
            if (!userIds.Contains(User.GetId()))
            {
                userIds.Add(User.GetId());
            }

            var newTrip = new Trip(dto.Name);
            newTrip.Users = await _repository.User.GetAll()
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            _repository.Trip.Add(newTrip);
            await _repository.SaveAsync();

            return Ok(new TripDto(newTrip));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TripDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTrips()
        {
            var userId = User.GetId();
            var trips = await _repository.Trip.GetAll()
                .Where(t => t.Users.Select(u => u.Id).Contains(userId))
                .Include(t => t.Users)
                .ToListAsync();
            return Ok(trips.Select(t => new TripDto(t)));
        }

        [HttpGet("{tripId}")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid tripId)
        {
            var userId = User.GetId();
            var trip = await _repository.Trip.GetAll()
                .Where(t => t.Id == tripId)
                .Where(t => t.Users.Select(u => u.Id).Contains(userId))
                .Include(t => t.Users)
                .FirstOrDefaultAsync();
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(new TripDto(trip));
        }

        [HttpPost("addUsers")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUsersToTrip(TripAddUsersDto dto)
        {
            await dto.Validate().ThrowIfInvalid();
            var userId = User.GetId();
            var trip = await _repository.Trip.GetAll()
                .Where(t => t.Id == dto.TripId)
                .Where(t => t.Users.Select(u => u.Id).Contains(userId))
                .Include(t => t.Users)
                .FirstOrDefaultAsync();

            if (trip == null)
            {
                return NotFound();
            }

            var users = await _repository.User.GetAll()
                .Where(u => dto.UserIds.Contains(u.Id))
                .ToListAsync();

            var userIdsToAdd = dto.UserIds.Except(trip.Users.Select(u => u.Id));
            var usersToAdd = users.Where(u => userIdsToAdd.Contains(u.Id));
            foreach (var user in usersToAdd)
            {
                trip.Users.Add(user);
            }

            await _repository.SaveAsync();
            return Ok(new TripDto(trip));
        }

        [HttpDelete("{tripId}/removeUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveUserFromTrip(Guid tripId, Guid userId)
        {
            var trip = await _repository.Trip.GetAll()
                    .Where(t => t.Id == tripId)
                    .Include(t => t.Users)
                    .Where(t => t.Users.Select(u => u.Id).Contains(userId))
                    .FirstOrDefaultAsync();

            if (trip == null)
            {
                return NotFound();
            }

            var user = trip.Users.Where(u => u.Id == userId).FirstOrDefault();
            trip.Users.Remove(user);
            await _repository.SaveAsync();
            return Ok();
        }

    }
}
