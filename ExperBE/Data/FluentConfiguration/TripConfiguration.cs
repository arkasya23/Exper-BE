using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExperBE.Data.FluentConfiguration
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
