using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Dtos.Users;
using ExperBE.Models.Entities;

namespace ExperBE.Dtos.PersonalExpenses
{
    public class PersonalExpenseDto
    {
        public Guid Id { get; }
        public string Description { get; }
        public decimal Amount { get; }
        public Guid TripId { get; }
        public UserDto CreatedBy { get; }

        public PersonalExpenseDto(PersonalExpense expense)
        {
            Id = expense.Id;
            Description = expense.Description;
            Amount = expense.Amount;
            TripId = expense.TripId;
            CreatedBy = expense.CreatedBy != null ? new UserDto(expense.CreatedBy) : null!;
        }
    }
}
