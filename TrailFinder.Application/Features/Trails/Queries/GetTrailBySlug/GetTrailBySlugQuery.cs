using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;

public record GetTrailBySlugQuery(string Slug) : IRequest<TrailDetailDto?>;