// TrailFinder.Core/DTOs/Trails/TrailFilterDto.cs

using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails;

public record TrailFilterDto(
    string? SearchTerm = null,
    Guid? ParentId = null,
    decimal? MinDistance = null,
    decimal? MaxDistance = null,
    decimal? MinElevation = null,
    decimal? MaxElevation = null,
    DifficultyLevel? DifficultyLevel = null,
    bool IncludeRouteGeometry = false
) : PageRequest;