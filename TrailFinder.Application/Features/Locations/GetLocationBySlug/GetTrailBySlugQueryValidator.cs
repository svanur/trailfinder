using FluentValidation;

namespace TrailFinder.Application.Features.Locations.GetLocationBySlug;

public class GetLocationBySlugQueryValidator : AbstractValidator<GetLocationBySlugQuery>
{
    public GetLocationBySlugQueryValidator()
    {
        RuleFor(v => v.Slug)
            .NotEmpty()
            .MaximumLength(200)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens");
    }
}