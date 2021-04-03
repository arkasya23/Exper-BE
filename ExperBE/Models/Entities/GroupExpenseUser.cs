using System;
using ExperBE.Models.Entities.Base;

namespace ExperBE.Models.Entities
{
    public class GroupExpenseUser : BaseEntity
    {
        public Guid UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public Guid GroupExpenseId { get; set; } = default!;
        public GroupExpense GroupExpense { get; set; } = default!;


        public GroupExpenseUser(Guid userId)
        {
            UserId = userId;
        }

        public GroupExpenseUser(Guid userId, Guid groupExpenseId)
        {
            UserId = userId;
            GroupExpenseId = groupExpenseId;
        }
    }
}
