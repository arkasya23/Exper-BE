using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExperBE.Data.FluentConfiguration
{
    public class GroupExpenseUserConfiguration : IEntityTypeConfiguration<GroupExpenseUser>
    {
        public void Configure(EntityTypeBuilder<GroupExpenseUser> builder)
        {
            builder.HasKey(gu => gu.Id);
            builder.HasOne(gu => gu.GroupExpense)
                .WithMany(g => g.Users)
                .HasForeignKey(gu => gu.GroupExpenseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(gu => gu.User)
                .WithMany()
                .HasForeignKey(gu => gu.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasIndex(gu => new { gu.GroupExpenseId, gu.UserId }).IsUnique();
        }
    }
}
