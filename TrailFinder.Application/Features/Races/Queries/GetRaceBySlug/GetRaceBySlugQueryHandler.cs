using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Races.Queries.GetRaceBySlug;

public class GetRaceBySlugQueryHandler : IRequestHandler<GetRaceBySlugQuery, RaceDto?>
{
    private readonly IMapper _mapper;
    private readonly IRaceRepository _raceRepository;

    public GetRaceBySlugQueryHandler(
        IRaceRepository raceRepository,
        IMapper mapper)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
    }

    public async Task<RaceDto?> Handle(GetRaceBySlugQuery request, CancellationToken cancellationToken)
    {
        var race = await _raceRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (race == null) throw new RaceNotFoundException(request.Slug);

        return _mapper.Map<RaceDto>(race);
    }
}