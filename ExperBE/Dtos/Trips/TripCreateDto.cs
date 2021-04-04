using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Dtos.Trips
{
    public class TripCreateDto
    {
        public string Name { get; set; } = null!;
        public IEnumerable<Guid>? UserIds { get; set; } = null!;

        public Task<ValidationResult> Validate() => new TripCreateDtoValidator().ValidateAsync(this);
        private class TripCreateDtoValidator : AbstractValidator<TripCreateDto>
        {
            public TripCreateDtoValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
                RuleFor(x => x.Name).MaximumLength(255).WithMessage("Name must be shorter than 256 characters");
            }
        }
    }
}
