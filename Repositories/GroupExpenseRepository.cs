using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Data;
using ExperBE.Models.Entities;
using ExperBE.Repositories.Interfaces;

namespace ExperBE.Repositories
{
    public class GroupExpenseRepository : BaseRepository<GroupExpense>, IGroupExpenseRepository
    {
        public GroupExpenseRepository(ExperDbContext context) : base(context)
        {
        }
    }
}
