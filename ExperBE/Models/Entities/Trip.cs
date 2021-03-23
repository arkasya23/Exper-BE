using System;
using System.Collections.Generic;
using ExperBE.Models.Entities.Base;

namespace ExperBE.Models.Entities
{
    public class Trip : BaseEntity
    {
        public ICollection<User> Users { get; set; } = default!;
    }
}
