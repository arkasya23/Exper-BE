using System;
using System.Collections.Generic;
using ExperBE.Models.Entities.Base;

namespace ExperBE.Models.Entities
{
    public class GroupExpense : BaseEntity
    {
        public string Description { get; set; } = "";
        public decimal Amount { get; set; } = 0.0m;
        public bool DivideBetweenAllMembers { get; set; } = false;
        public Guid CreatedById { get; set; } = default!;
        public User CreatedBy { get; set; } = default!;
        public Guid TripId { get; set; } = default!;
        public Trip Trip { get; set; } = default!;
        public ICollection<GroupExpenseUser> Users { get; set; } = default!;

        public GroupExpense(string description, decimal amount, bool divideBetweenAllMembers, Guid createdById, Guid tripId)
        {
            Description = description;
            Amount = amount;
            DivideBetweenAllMembers = divideBetweenAllMembers;
            CreatedById = createdById;
            TripId = tripId;
        }
    }
}
