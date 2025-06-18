// TrailFinder.Core/DTOs/Trails/TrailFilterDto.cs

using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails;

public record TrailFilterDto(
    string? SearchTerm = null,
    Guid? ParentId = null,
    double? MinDistance = null,
    double? MaxDistance = null,
    double? MinElevation = null,
    double? MaxElevation = null,
    DifficultyLevel? DifficultyLevel = null,
    bool IncludeRouteGeom = false
) : PageRequest;