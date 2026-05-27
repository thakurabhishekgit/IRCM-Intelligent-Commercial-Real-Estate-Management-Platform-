using FluentValidation;
using IRCM.DTOs.LeaseRequest;

namespace IRCM.Validators.LeaseRequest;

public class CreateLeaseRequestValidator
    : AbstractValidator<CreateLeaseRequestDto>
{
    public CreateLeaseRequestValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(500);
    }
}