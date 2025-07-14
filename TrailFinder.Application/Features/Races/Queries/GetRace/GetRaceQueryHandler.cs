using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Races.Queries.GetRace;

public class GetRaceQueryHandler : IRequestHandler<GetRaceQuery, RaceDto>
{
    private readonly ILogger<GetRaceQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRaceRepository _raceRepository;

    public GetRaceQueryHandler(
        ILogger<GetRaceQueryHandler> logger,
        IMapper mapper,
        IRaceRepository raceRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _raceRepository = raceRepository;
    }

    public async Task<RaceDto> Handle(GetRaceQuery request, CancellationToken cancellationToken)
    {
        // Use the new method that includes related data
        var race = await _raceRepository.GetByIdWithLocationsAsync(request.Id, cancellationToken);
        if (race == null)
        {
            throw new RaceNotFoundException(request.Id);
        }

        return _mapper.Map<RaceDto>(race);
    }
}