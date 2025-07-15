using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandler(
    ITrailRepository trailRepository,
    IMapper mapper
) : IRequestHandler<GetTrailQuery, TrailDto>
{
    public async Task<TrailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        var trail = await trailRepository.GetByIdAsync(request.Id, cancellationToken);

        if (trail == null) throw new TrailNotFoundException(request.Id);

        return mapper.Map<TrailDto>(trail);
    }
}