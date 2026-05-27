using FluentValidation;
using IRCM.DTOs.Lease;

namespace IRCM.Validators.Lease;

public class CreateLeaseValidator
    : AbstractValidator<CreateLeaseDto>
{
    public CreateLeaseValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage(
                "PropertyId is required"
            );

        RuleFor(x => x.TenantId)
            .NotEmpty()
            .WithMessage(
                "TenantId is required"
            );

        RuleFor(x => x.LeaseRequestId)
            .NotEmpty()
            .WithMessage(
                "LeaseRequestId is required"
            );

        RuleFor(x => x.MonthlyRent)
            .GreaterThan(0)
            .WithMessage(
                "Monthly rent must be greater than 0"
            );

        RuleFor(x => x.SecurityDeposit)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Security deposit cannot be negative"
            );

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(
                "Start date must be future date"
            );

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage(
                "End date must be greater than start date"
            );
    }
}