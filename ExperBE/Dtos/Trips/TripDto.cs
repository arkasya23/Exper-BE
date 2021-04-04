using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Models.Entities;

namespace ExperBE.Dtos.Trips
{
    public class TripDto
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<TripUserDto> Users { get; }

        public TripDto(Trip trip)
        {
            Id = trip.Id;
            Name = trip.Name;
            Users = trip.Users.Select(u => new TripUserDto(u));
        }

        public class TripUserDto
        {
            public Guid Id { get; }
            public string Email { get; }

            public TripUserDto(User user)
            {
                Id = user.Id;
                Email = user.Email;
            }
        }
    }

}
