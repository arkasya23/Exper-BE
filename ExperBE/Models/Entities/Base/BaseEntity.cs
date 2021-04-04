using System;

namespace ExperBE.Models.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = default!;
    }
}
