using System;
using System.Collections.Generic;
using ExperBE.Models.Entities.Base;

namespace ExperBE.Models.Entities
{
    public class Trip : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CreatedByUserId { get; set; } = default!;
        public User CreatedByUser { get; set; } = default!;
        public ICollection<User> Users { get; set; } = default!;
        public ICollection<GroupExpense> GroupExpenses { get; set; } = default!;
        public ICollection<PersonalExpense> PersonalExpenses { get; set; } = default!;

        public Trip(string name, Guid createdByUserId)
        {
            Name = name;
            CreatedByUserId = createdByUserId;
        }
    }
}
