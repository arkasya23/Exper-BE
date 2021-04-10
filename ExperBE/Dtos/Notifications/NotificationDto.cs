using System;
using ExperBE.Models.Entities;

namespace ExperBE.Dtos.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        public NotificationDto(Notification notification)
        {
            Id = notification.Id;
            Title = notification.Title;
            Description = notification.Description;
            CreatedAt = notification.CreatedAt;
        }
    }
}
