using FluentValidation;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;

public class GetTrailsByParentIdQueryValidator : AbstractValidator<GetTrailsByParentIdQuery>
{
    public GetTrailsByParentIdQueryValidator()
    {
        RuleFor(t => t.ParentId)
            .NotEmpty().WithMessage("Parent ID cannot be empty")
            .Must(id => id != Guid.Empty).WithMessage("Parent ID cannot be an empty GUID")
            .Must(BeValidGuid).WithMessage("Parent ID must be a valid GUID format");
    }

    private static bool BeValidGuid(Guid parentId)
    {
        // Additional custom validation, if needed
        // For example, checking if it follows a specific format or pattern
        return parentId.ToString().Length == 36; // Standard GUID length
    }
}
