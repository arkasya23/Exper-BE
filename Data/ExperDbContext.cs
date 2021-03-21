using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExperBE.Data
{
    public class ExperDbContext : DbContext
    {
        public ExperDbContext(DbContextOptions<ExperDbContext> options) : base(options)
        {
        }

        public DbSet<GroupExpense> GroupExpenses { get; set; } = null!;
        public DbSet<GroupExpenseUser> GroupExpenseUsers { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<PersonalExpense> PersonalExpenses { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExperDbContext).Assembly);
        }
    }
}
