using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        var paginatedTrails = await _trailRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.SortBy,
            request.SortDescending,
            cancellationToken
        );

        // AutoMapper setup for PaginatedResult<TSource> to PaginatedResult<TDestination>
        // ensures that all properties (Items, PageNumber, etc.) are correctly mapped.
        return _mapper.Map<PaginatedResult<TrailDto>>(paginatedTrails);
    }
}