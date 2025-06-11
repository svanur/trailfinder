using FluentValidation;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;

public class GetTrailsByParentIdQueryValidator : AbstractValidator<GetTrailsByParentIdQuery>
{
    public GetTrailsByParentIdQueryValidator()
    {
        RuleFor(v => v.ParentId)
            .NotEmpty();
    }
}
