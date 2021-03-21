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
        }
    }
}
