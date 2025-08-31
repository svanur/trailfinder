// TrailFinder.Application/Features/GpxFiles/Commands/CreateGpxFileMetadata/CreateGpxFileMetadataCommand.cs
using MediatR;
using NetTopologySuite.Geometries; // If you plan to store derived RouteGeom here

namespace TrailFinder.Application.Features.GpxFiles.Commands.CreateGpxFileMetadata;

public record CreateGpxFileMetadataCommand(
    Guid TrailId,
    string StoragePath,
    string OriginalFileName,
    string FileName, // The sanitized file name used in storage_path
    long FileSize,
    string ContentType,
    Guid CreatedBy // The ID of the user who uploaded the file
    // You might also pass extracted GPX info here if you process it during upload:
    // double? DistanceMeters,
    // double? ElevationGainMeters,
    // DifficultyLevel? DifficultyLevel,
    // RouteType? RouteType,
    // TerrainType? TerrainType,
    // SurfaceType? SurfaceType,
    // LineString? RouteGeom
) : IRequest<Guid>; // Returns the ID of the new gpx_file metadata record
