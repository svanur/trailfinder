using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Trails.Requests;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.CreateTrail;

public record CreateTrailCommand : IRequest<Guid>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Distance { get; init; }
    public decimal ElevationGain { get; init; }
    
    public DifficultyLevel? DifficultyLevel { get; init; }
    public RouteType? RouteType { get; init; }
    public TerrainType? TerrainType { get; init; }
    public SurfaceType? SurfaceType { get; init; }
    
    public LineString? RouteGeom { get; init; }
    
    public string? WebUrl { get; init; }
    public Guid CreatedBy { get; init; }
}