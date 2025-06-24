using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

public class GetTrailsQueryHandler : IRequestHandler<GetTrailsQuery, PaginatedResult<TrailDto>>
{
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;

    public GetTrailsQueryHandler(
        ITrailRepository trailRepository,
        IMapper mapper)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<TrailDto>> Handle(
        GetTrailsQuery request,
        CancellationToken cancellationToken)
    {
        var trails = await _trailRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<PaginatedResult<TrailDto>>(trails);
    }
}