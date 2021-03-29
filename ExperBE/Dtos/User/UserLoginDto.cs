using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Dtos.User
{
    public class UserLoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public Task<ValidationResult> Validate() => new UserLoginDtoValidator().ValidateAsync(this);
        private class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
        {
            public UserLoginDtoValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("Email must not be empty");
                RuleFor(x => x.Email).EmailAddress().WithMessage("Value must be an email address");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password must not be empty");
            }
        }
    }
}
