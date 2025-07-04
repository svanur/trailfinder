using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Application.Features.Trails.Commands.UpdateTrail;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

public class GetTrailsQueryHandler : IRequestHandler<GetTrailsQuery, PaginatedResult<TrailDto>>
{
    private readonly ILogger<GetTrailsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;

    public GetTrailsQueryHandler(
        ILogger<GetTrailsQueryHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
    }

    public async Task<PaginatedResult<TrailDto>> Handle(
        GetTrailsQuery request,
        CancellationToken cancellationToken)
    {
        var trails = await _trailRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<PaginatedResult<TrailDto>>(trails);
    }
}