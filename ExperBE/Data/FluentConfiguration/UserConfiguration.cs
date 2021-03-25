using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExperBE.Data.FluentConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();
            builder.HasIndex(u => u.Email)
                .IsUnique();
            builder.HasMany(u => u.Trips)
                .WithMany(t => t.Users);
        }
    }
}
