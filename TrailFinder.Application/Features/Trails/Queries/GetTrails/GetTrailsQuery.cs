// TrailFinder.Application.Features.Trails.Queries.GetTrails/GetTrailsQuery.cs

using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

// Make sure these properties are part of your GetTrailsQuery record
public class GetTrailsQuery : IRequest<List<TrailListItemDto>>
{
    public double? UserLatitude { get; set; }
    public double? UserLongitude { get; set; }
}