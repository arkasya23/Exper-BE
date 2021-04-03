using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Dtos.Users
{
    public class UserRegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public Task<ValidationResult> Validate() => new UserRegisterDtoValidator().ValidateAsync(this);
        private class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
        {
            public UserRegisterDtoValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("Email must not be empty");
                RuleFor(x => x.Email).EmailAddress().WithMessage("Value must be an email address");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password must not be empty");
                RuleFor(x => x.Password).MinimumLength(4).WithMessage("Password must be 4 characters or longer");
            }
        }
    }

}
