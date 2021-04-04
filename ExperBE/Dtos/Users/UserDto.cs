using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;

namespace ExperBE.Dtos.Users
{
    public class UserDto
    {
        public Guid Id { get; }
        public string Email { get; }

        public UserDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
        }
    }
}
