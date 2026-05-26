using FluentValidation;
using IRCM.DTOs.Property;

namespace IRCM.Validators.Property;

public class CreatePropertyValidator
    : AbstractValidator<CreatePropertyDto>
{
    public CreatePropertyValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Location)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.TotalUnits)
            .GreaterThan(0);

        RuleFor(x => x.OccupiedUnits)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Amenities)
            .NotEmpty();

        RuleFor(x => x.ThumbnailUrl)
            .NotEmpty();

        RuleFor(x => x.PropertyType)
            .IsInEnum();
    }
}