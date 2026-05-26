using FluentValidation;
using IRCM.DTOs.Auth;

namespace IRCM.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^[0-9]{10}$");

        // RuleFor(x => x.Role)
        //     .NotEmpty()
        //     .Must(role =>
        //         role == "Admin" ||
        //         role == "Agent" ||
        //         role == "Tenant"
        //     )
        //     .WithMessage("Invalid role.");
    }
}