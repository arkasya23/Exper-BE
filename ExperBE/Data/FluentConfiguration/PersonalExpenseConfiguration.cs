using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExperBE.Data.FluentConfiguration
{
    public class PersonalExpenseConfiguration : IEntityTypeConfiguration<PersonalExpense>
    {
        public void Configure(EntityTypeBuilder<PersonalExpense> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
