using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExperBE.Data.FluentConfiguration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Title)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(n => n.Description)
                .HasMaxLength(2047)
                .IsRequired();
            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
