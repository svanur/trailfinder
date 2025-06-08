using FluentValidation;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;

public class GetTrailBySlugQueryValidator : AbstractValidator<GetTrailBySlugQuery>
{
    public GetTrailBySlugQueryValidator()
    {
        RuleFor(v => v.Slug)
            .NotEmpty()
            .MaximumLength(200)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens");
    }
}
