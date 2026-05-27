namespace IRCM.Validations;
using FluentValidation;
using IRCM.DTOs.LeaseRequest;

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