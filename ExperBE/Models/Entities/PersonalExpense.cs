using System;
using ExperBE.Models.Entities.Base;

namespace ExperBE.Models.Entities
{
    public class PersonalExpense : BaseEntity
    {
        public string Description { get; set; } = "";
        public decimal Amount { get; set; } = 0.0m;
        public Guid CreatedById { get; set; } = default!;
        public User CreatedBy { get; set; } = default!;
        public Guid TripId { get; set; } = default!;
        public Trip Trip { get; set; } = default!;

        public PersonalExpense(string description, decimal amount, Guid createdById, Guid tripId)
        {
            Description = description;
            Amount = amount;
            CreatedById = createdById;
            TripId = tripId;
        }
    }
}
