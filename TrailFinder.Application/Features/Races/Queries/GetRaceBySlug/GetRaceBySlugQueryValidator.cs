using FluentValidation;

namespace TrailFinder.Application.Features.Races.Queries.GetRaceBySlug;

public class GetRaceBySlugQueryValidator : AbstractValidator<GetRaceBySlugQuery>
{
    public GetRaceBySlugQueryValidator()
    {
        RuleFor(v => v.Slug)
            .NotEmpty()
            .MaximumLength(200)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens");
    }
}