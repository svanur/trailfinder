using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Exceptions;
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
        
        var races = await _raceRepository.GetAllAsync(cancellationToken);
        if (races == null)
        {
            throw new RaceNotFoundException();
        }

        return _mapper.Map<PaginatedResult<RaceDto>>(races);
    }
}