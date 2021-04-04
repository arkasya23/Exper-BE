using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Dtos.Trips
{
    public class TripAddUsersDto
    {
        public Guid TripId { get; set; }
        public IEnumerable<Guid> UserIds { get; set; } = null!;

        public Task<ValidationResult> Validate() => new TripAddUsersDtoValidator().ValidateAsync(this);
        private class TripAddUsersDtoValidator : AbstractValidator<TripAddUsersDto>
        {
            public TripAddUsersDtoValidator()
            {
                RuleFor(x => x.TripId).NotEmpty().WithMessage("A trip must be selected to add users");
                RuleFor(x => x.UserIds).NotEmpty().WithMessage("At least one user must be added");
            }
        }
    }
}
