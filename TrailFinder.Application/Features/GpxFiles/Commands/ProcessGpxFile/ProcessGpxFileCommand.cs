// TrailFinder.Application/Features/GpxFiles/Commands/ProcessGpxFile/ProcessGpxFileCommand.cs

using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;
// For GpxAnalysisResult

namespace TrailFinder.Application.Features.GpxFiles.Commands.ProcessGpxFile;

public record ProcessGpxFileCommand(
    // Data for GpxFile metadata table
    Guid TrailId,
    string StoragePath,
    string OriginalFileName,
    string FileName,
    long FileSize,
    string ContentType,
    Guid CreatedBy, // Or UpdatedBy if it's a replacement
    
    // Data for updating the Trail entity, derived from GpxAnalysisResult
    double AnalyzedDistance,
    double AnalyzedElevationGain,
    DifficultyLevel AnalyzedDifficultyLevel,
    RouteType AnalyzedRouteType,
    TerrainType AnalyzedTerrainType,
    LineString? AnalyzedRouteGeom // The NTS Geometry for the trail
) : IRequest<Guid>; // Returns the GpxFileMetadataId