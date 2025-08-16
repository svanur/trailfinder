using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;

public record GetTrailBySlugQuery : IRequest<TrailDetailDto?>
{
    public string Slug { get; set; } = string.Empty;
    public double? UserLatitude { get; set; }
    public double? UserLongitude { get; set; }
    public double? UserElevation { get; set; }
}