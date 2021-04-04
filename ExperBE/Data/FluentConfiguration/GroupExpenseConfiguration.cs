using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExperBE.Data.FluentConfiguration
{
    public class GroupExpenseConfiguration : IEntityTypeConfiguration<GroupExpense>
    {
        public void Configure(EntityTypeBuilder<GroupExpense> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description)
                .HasMaxLength(2047)
                .IsRequired();
            builder.Property(e => e.Amount)
                .HasPrecision(19, 4)
                .IsRequired();
            builder.HasOne(e => e.Trip)
                .WithMany(t => t.GroupExpenses)
                .HasForeignKey(e => e.TripId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
