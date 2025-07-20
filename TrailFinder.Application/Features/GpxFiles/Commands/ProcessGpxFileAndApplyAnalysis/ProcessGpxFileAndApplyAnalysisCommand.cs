// TrailFinder.Application/Features/GpxFiles/Commands/ProcessGpxFileAndApplyAnalysis/ProcessGpxFileAndApplyAnalysisCommand.cs
using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx.Responses; // For GpxInfoDto
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.GpxFiles.Commands.ProcessGpxFileAndApplyAnalysis;

public record ProcessGpxFileAndApplyAnalysisCommand(
    // Data for GpxFile metadata table
    Guid TrailId,
    string StoragePath,
    string OriginalFileName,
    string FileName,
    long FileSize,
    string ContentType,
    Guid CreatedBy, // Or UpdatedBy if it's a replacement
    
    // Data for updating the Trail entity, derived from GpxInfoDto
    double AnalyzedDistance,
    double AnalyzedElevationGain,
    DifficultyLevel AnalyzedDifficultyLevel,
    RouteType AnalyzedRouteType,
    TerrainType AnalyzedTerrainType,
    LineString? AnalyzedRouteGeom // The NTS Geometry for the trail
) : IRequest<Guid>; // Returns the GpxFileMetadataId