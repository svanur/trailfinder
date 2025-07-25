using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrailGpxInfo;

public record UpdateTrailGpxInfoCommand(
    Guid TrailId,
    double? Distance,
    double? ElevationGain,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    LineString? RouteGeom

) : IRequest<Unit>;
