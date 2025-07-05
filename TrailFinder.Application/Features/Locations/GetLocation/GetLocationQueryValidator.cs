using FluentValidation;

namespace TrailFinder.Application.Features.Locations.GetLocation;

public class GetLocationQueryValidator : AbstractValidator<GetLocationQuery>
{
    public GetLocationQueryValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty().WithMessage("Location ID cannot be empty")
            .Must(id => id != Guid.Empty).WithMessage("Location ID cannot be an empty GUID")
            .Must(BeValidGuid).WithMessage("Location ID must be a valid GUID format");
    }

    private static bool BeValidGuid(Guid id)
    {
        // Additional custom validation, if needed
        // For example, checking if it follows a specific format or pattern
        return id.ToString().Length == 36; // Standard GUID length
    }
}