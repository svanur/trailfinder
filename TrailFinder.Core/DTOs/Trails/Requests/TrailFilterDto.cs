using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record TrailFilterDto(
    string? SearchTerm = null,
    double? MinDistance = null,
    double? MaxDistance = null,
    double? MinElevation = null,
    double? MaxElevation = null,
    DifficultyLevel? DifficultyLevel = null,
    //TODO: Add RouteType & TerrainType ?
    bool IncludeRouteGeom = false
) : PageRequest;