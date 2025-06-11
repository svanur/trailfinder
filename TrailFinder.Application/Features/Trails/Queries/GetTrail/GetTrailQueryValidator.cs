using FluentValidation;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryValidator : AbstractValidator<GetTrailQuery>
{
    public GetTrailQueryValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty().WithMessage("Trail ID cannot be empty")
            .Must(id => id != Guid.Empty).WithMessage("Trail ID cannot be an empty GUID")
            .Must(BeValidGuid).WithMessage("Trail ID must be a valid GUID format");
    }

    private static bool BeValidGuid(Guid id)
    {
        // Additional custom validation, if needed
        // For example, checking if it follows a specific format or pattern
        return id.ToString().Length == 36; // Standard GUID length
    }

}
