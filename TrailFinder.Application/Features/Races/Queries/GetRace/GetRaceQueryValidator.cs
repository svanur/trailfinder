using FluentValidation;

namespace TrailFinder.Application.Features.Races.Queries.GetRace;

public class GetRaceQueryValidator : AbstractValidator<GetRaceQuery>
{
    public GetRaceQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Race ID cannot be empty")
            .Must(id => id != Guid.Empty).WithMessage("Race ID cannot be an empty GUID")
            .Must(BeValidGuid).WithMessage("Race ID must be a valid GUID format");
    }

    private static bool BeValidGuid(Guid id)
    {
        // Additional custom validation, if needed
        // For example, checking if it follows a specific format or pattern
        return id.ToString().Length == 36; // Standard GUID length
    }
}