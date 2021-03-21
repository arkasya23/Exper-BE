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
        }
    }
}
