using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Dtos.GroupExpenses;
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
    public class GroupExpensesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public GroupExpensesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GroupExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGroupExpense(GroupExpenseCreateDto dto)
        {
            await dto.Validate().ThrowIfInvalid();
            var trip = await _repository.Trip.GetAll()
                .Where(t => t.Users.Select(u => u.Id).Contains(User.GetId()))
                .Where(t => t.Id == dto.TripId)
                .Include(t => t.Users)
                .FirstOrDefaultAsync();

            if (trip == null)
            {
                return NotFound();
            }


            var newGroupExpense = new GroupExpense(dto.Description, dto.Amount, dto.DivideBetweenAllMembers, User.GetId(), dto.TripId);
            newGroupExpense.Users = new List<GroupExpenseUser>();

            var shareAmount = newGroupExpense.DivideBetweenAllMembers
                ? newGroupExpense.Amount / trip.Users.Count
                : newGroupExpense.Amount / dto.UserIds.Count();
            shareAmount = Math.Round(shareAmount, 2);

            if (newGroupExpense.DivideBetweenAllMembers)
            {
                foreach (var user in trip.Users)
                {
                    if (user.Id == User.GetId()) continue;

                    _repository.Notification.Add(new Notification($"New group expense added",
                        $"A new group expense was added for the trip {trip.Name}! It totals {newGroupExpense.Amount} and your share of {shareAmount} must be paid to {User.GetEmail()}!",
                        user.Id));
                }
            }
            else
            {
                var usersInTrip = trip.Users.Select(u => u.Id);
                dto.UserIds
                    .Where(id => usersInTrip.Contains(id))
                    .ToList()
                    .ForEach(id =>
                    {
                        newGroupExpense.Users.Add(new GroupExpenseUser(id));
                        if (id != User.GetId())
                        {
                            _repository.Notification.Add(new Notification($"New group expense added",
                                $"A new group expense was added for the trip {trip.Name}! It totals {dto.Amount} and your share of {shareAmount} must be paid to {User.GetEmail()}!",
                                id));
                        }
                    });
            }

            _repository.GroupExpense.Add(newGroupExpense);
            await _repository.SaveAsync();

            var result = new GroupExpenseDto(newGroupExpense);
            return Ok(result);
        }

        [HttpGet("trip/{tripId}")]
        [ProducesResponseType(typeof(IEnumerable<GroupExpenseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGroupExpensesByTripId(Guid tripId)
        {
            var expenses = await _repository.GroupExpense.GetAll().AsNoTracking()
                .Where(e => e.Trip.Users.Select(u => u.Id).Contains(User.GetId()))
                .Where(e => e.TripId == tripId)
                .Include(e => e.CreatedBy)
                .Include(e => e.Users)
                    .ThenInclude(ou => ou.User)
                .ToListAsync();
            var result = expenses.Select(e => new GroupExpenseDto(e));
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GroupExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGroupExpense(GroupExpenseUpdateDto dto, Guid id)
        {
            await dto.Validate().ThrowIfInvalid();
            var expense = await _repository.GroupExpense.GetAll()
                .Where(e => e.Trip.Users.Select(u => u.Id).Contains(User.GetId()))
                .Where(e => e.Id == id)
                .Include(e => e.CreatedBy)
                .Include(e => e.Trip)
                    .ThenInclude(t => t.Users)
                .Include(e => e.Users)
                    .ThenInclude(ou => ou.User)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound();
            }

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.DivideBetweenAllMembers = dto.DivideBetweenAllMembers;

            var shareAmount = expense.DivideBetweenAllMembers
                ? expense.Amount / expense.Trip.Users.Count
                : expense.Amount / dto.UserIds.Count();
            shareAmount = Math.Round(shareAmount, 2);

            if (expense.DivideBetweenAllMembers)
            {
                // Remove all users
                foreach (var groupExpenseUser in expense.Users)
                {
                    _repository.GroupExpenseUser.Remove(groupExpenseUser);
                }

                // Send notifications
                foreach (var user in expense.Trip.Users)
                {
                    if (user.Id == expense.CreatedById) continue;

                    _repository.Notification.Add(new Notification($"A group expense was updated",
                        $"A group expense for the trip {expense.Trip.Name} was updated! It totals {dto.Amount} and your share of {shareAmount} must be paid to {expense.CreatedBy.Email}!",
                        user.Id));
                }
            }
            else
            {
                var usersToRemove = expense.Users
                    .Select(ou => ou.UserId)
                    .Except(dto.UserIds);
                var usersToAdd = dto.UserIds
                    .Except(expense.Users.Select(ou => ou.UserId))
                    .ToList();

                foreach (var groupExpenseUser in expense.Users.Where(u => usersToRemove.Contains(u.UserId)))
                {
                    _repository.GroupExpenseUser.Remove(groupExpenseUser);
                }

                usersToAdd.ForEach(userId => expense.Users.Add(new GroupExpenseUser(userId)));

                expense.Users.Where(u => !usersToRemove.Contains(u.UserId)).ToList().ForEach(e =>
                {
                    _repository.Notification.Add(new Notification($"A group expense was updated",
                        $"A group expense for the trip {expense.Trip.Name} was updated! It totals {dto.Amount} and your share of {shareAmount} must be paid to {expense.CreatedBy.Email}!",
                        e.UserId));
                });
            }

            await _repository.SaveAsync();
            var result = new GroupExpenseDto(expense);
            return Ok(result);
        }

    }
}
