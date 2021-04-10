using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Dtos.GroupExpenses
{
    public class GroupExpenseUpdateDto
    {
        public string Description { get; set; } = default!;
        public decimal Amount { get; set; } = default!;
        public bool DivideBetweenAllMembers { get; set; } = default!;
        public IEnumerable<Guid> UserIds { get; set; } = default!;

        public Task<ValidationResult> Validate() => new GroupExpenseUpdateDtoValidator().ValidateAsync(this);
        private class GroupExpenseUpdateDtoValidator : AbstractValidator<GroupExpenseUpdateDto>
        {
            public GroupExpenseUpdateDtoValidator()
            {
                RuleFor(x => x.Description).NotEmpty().WithMessage("Description must not be empty!");
                RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount must not be empty!");
                RuleFor(x => x.UserIds).NotEmpty().Unless(x => x.DivideBetweenAllMembers).WithMessage("Must specify userIds when not dividing between all members!");
            }
        }
    }
}
