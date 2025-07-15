using MediatR;
using TrailFinder.Core.DTOs.Race.Response;

namespace TrailFinder.Application.Features.Races.Queries.GetRaceBySlug;

public record GetRaceBySlugQuery(string Slug) : IRequest<RaceDto?>;