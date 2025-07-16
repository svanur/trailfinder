using MediatR;
using TrailFinder.Core.DTOs.Race.Response;

namespace TrailFinder.Application.Features.Races.Queries.GetRace;

public record GetRaceQuery(Guid Id) : IRequest<RaceDto>;