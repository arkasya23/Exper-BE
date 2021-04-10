using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Dtos.Users;
using ExperBE.Models.Entities;

namespace ExperBE.Dtos.GroupExpenses
{
    public class GroupExpenseDto
    {
        public Guid Id { get; }
        public string Description { get; }
        public decimal Amount { get; }
        public bool DivideBetweenAllMembers { get; }
        public Guid TripId { get; }
        public UserDto CreatedBy { get; }
        public IEnumerable<UserDto> Users { get; }

        public GroupExpenseDto(GroupExpense expense)
        {
            Id = expense.Id;
            Description = expense.Description;
            Amount = expense.Amount;
            DivideBetweenAllMembers = expense.DivideBetweenAllMembers;
            TripId = expense.TripId;
            CreatedBy = expense.CreatedBy != null ? new UserDto(expense.CreatedBy) : null!;
            Users = expense.Users.Select(o => new UserDto(o.User));
        }
    }
}
