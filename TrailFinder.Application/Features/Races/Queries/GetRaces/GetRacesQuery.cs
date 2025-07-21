using MediatR;
using TrailFinder.Core.DTOs.Race.Response;

namespace TrailFinder.Application.Features.Races.Queries.GetRaces;

public record GetRacesQuery : IRequest<List<RaceDto>>;