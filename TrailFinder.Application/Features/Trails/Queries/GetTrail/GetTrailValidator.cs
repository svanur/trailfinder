using FluentValidation;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailValidator : AbstractValidator<GetTrailQuery>
{
    public GetTrailValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty();
    }
}
