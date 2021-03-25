using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;

namespace ExperBE.Extensions.IQueryable
{
    public static class UserExtensions
    {
        public static IQueryable<User> ForEmail(this IQueryable<User> users, string email)
            => users.Where(u => u.Email == email);
    }
}
