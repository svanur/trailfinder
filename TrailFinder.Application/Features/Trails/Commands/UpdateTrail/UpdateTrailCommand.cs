using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public record UpdateTrailCommand(
    Guid TrailId,
    double? Distance,
    double? ElevationGainMeters,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    LineString? RouteGeom

) : IRequest<Unit>;
