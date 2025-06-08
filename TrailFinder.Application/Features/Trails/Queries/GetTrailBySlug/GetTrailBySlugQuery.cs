using MediatR;
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;

public record GetTrailBySlugQuery(string Slug) : IRequest<TrailDto?>;
