using System;
using ExperBE.Models.Entities.Base;

namespace ExperBE.Models.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Notification(string title, string description, Guid userId)
        {
            Title = title;
            Description = description;
            UserId = userId;
        }
    }
}
