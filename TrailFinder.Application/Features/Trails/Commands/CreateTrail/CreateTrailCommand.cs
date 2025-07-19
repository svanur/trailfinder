using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.CreateTrail;

public record CreateTrailCommand(
    string Name,
    string? Description,
    double Distance,
    double ElevationGain,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    SurfaceType? SurfaceType,
    LineString? RouteGeom,
    string? WebUrl,
    Guid CreatedBy
) : IRequest<Guid>; // Returns the ID of the new gpx_file metadata record