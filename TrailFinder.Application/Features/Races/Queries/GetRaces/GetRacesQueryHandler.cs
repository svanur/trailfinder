using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Races.Queries.GetRaces;

public class GetRacesQueryHandler : IRequestHandler<GetRacesQuery, PaginatedResult<RaceDto>>
{
    private readonly ILogger<GetRacesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRaceRepository _raceRepository;

    public GetRacesQueryHandler(
        ILogger<GetRacesQueryHandler> logger,
        IMapper mapper,
        IRaceRepository raceRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _raceRepository = raceRepository;
    }

    public async Task<PaginatedResult<RaceDto>> Handle(
        GetRacesQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedTrails = await _raceRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.SortBy,
            request.SortDescending,
            cancellationToken
        );

        // AutoMapper setup for PaginatedResult<TSource> to PaginatedResult<TDestination>
        // ensures that all properties (Items, PageNumber, etc.) are correctly mapped.
        return _mapper.Map<PaginatedResult<RaceDto>>(paginatedTrails);
    }
}