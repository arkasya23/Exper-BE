using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Dtos.PersonalExpenses;
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
    public class PersonalExpensesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public PersonalExpensesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PersonalExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePersonalExpense(PersonalExpenseCreateDto dto)
        {
            await dto.Validate().ThrowIfInvalid();
            var trip = await _repository.Trip.GetAll()
                .Where(t => t.Users.Select(u => u.Id).Contains(User.GetId()))
                .Where(t => t.Id == dto.TripId)
                .FirstOrDefaultAsync();

            if (trip == null)
            {
                return NotFound();
            }

            var newPersonalExpense = new PersonalExpense(dto.Description, dto.Amount, User.GetId(), dto.TripId);
            _repository.PersonalExpense.Add(newPersonalExpense);
            await _repository.SaveAsync();

            var result = new PersonalExpenseDto(newPersonalExpense);
            return Ok(result);
        }

        [HttpGet("trip/{tripId}")]
        [ProducesResponseType(typeof(IEnumerable<PersonalExpenseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPersonalExpensesByTripId(Guid tripId)
        {
            var expenses = await _repository.PersonalExpense.GetAll().AsNoTracking()
                .Where(e => e.CreatedById == User.GetId())
                .Where(e => e.TripId == tripId)
                .ToListAsync();
            var result = expenses.Select(e => new PersonalExpenseDto(e));
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonalExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePersonalExpense(PersonalExpenseUpdateDto dto, Guid id)
        {
            await dto.Validate().ThrowIfInvalid();
            var expense = await _repository.PersonalExpense.GetAll()
                .Where(e => e.CreatedById == User.GetId())
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
            
            if (expense == null)
            {
                return NotFound();
            }

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            await _repository.SaveAsync();
            var result = new PersonalExpenseDto(expense);
            return Ok(result);
        }
    }
}