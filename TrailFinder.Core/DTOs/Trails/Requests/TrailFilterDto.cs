using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record TrailFilterDto(
    string? SearchTerm = null,
    decimal? MinDistance = null,
    decimal? MaxDistance = null,
    decimal? MinElevation = null,
    decimal? MaxElevation = null,
    DifficultyLevel? DifficultyLevel = null,
    RouteType? RouteType = null,
    TerrainType? TerrainType = null,
    bool IncludeRouteGeom = false
) : PageRequest;