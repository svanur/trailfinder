// TrailFinder.Application.Features.Trails.Queries.GetTrails/GetTrailsQuery.cs

using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

// Make sure these properties are part of your GetTrailsQuery record
public record GetTrailsQuery : IRequest<List<TrailDto>>;