using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Dtos.PersonalExpenses
{
    public class PersonalExpenseCreateDto
    {
        public string Description { get; set; } = default!;
        public decimal Amount { get; set; } = default!;
        public Guid TripId { get; set; } = default!;

        public Task<ValidationResult> Validate() => new PersonalExpenseCreateDtoValidator().ValidateAsync(this);

        private class PersonalExpenseCreateDtoValidator : AbstractValidator<PersonalExpenseCreateDto>
        {
            public PersonalExpenseCreateDtoValidator()
            {
                RuleFor(x => x.Description).NotEmpty().WithMessage("Description must not be empty");
                RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount must not be empty");
                RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
                RuleFor(x => x.TripId).NotEmpty().WithMessage("TripId must not be empty");
            }
        }
    }
}
