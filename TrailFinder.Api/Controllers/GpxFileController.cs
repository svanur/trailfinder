using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.GpxFiles.Commands.ProcessGpxFile;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Application.Features.GpxFiles.Queries.GetGpxFileMetadata;

namespace TrailFinder.Api.Controllers;

/// <summary>
/// Controller responsible for managing GPX files associated with trails.
/// Provides endpoints to upload, download, and retrieve metadata for GPX files.
/// </summary>
[ApiController]
[Route("api/trails/{trailId:guid}/gpx-file")] // Updated route to be more precise
public class GpxFileController(
    ILogger<BaseApiController> logger,
    IMediator mediator,
    ISupabaseStorageService storageService,
    IGpxService gpxService
) : BaseApiController(logger)
{

    [HttpPost("upload")] // Route: api/trails/{trailId}/gpx-file/upload
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> Upload(Guid trailId, IFormFile file)
    {
        try
        {
            if (file.Length == 0)
            {
                return BadRequest("No file was uploaded or file is empty.");
            }
            if (!file.FileName.EndsWith(".gpx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("File must be a GPX file.");
            }

            var trailDto = await mediator.Send(new GetTrailQuery(trailId));
            if (trailDto == null)
            {
                throw new TrailNotFoundException(trailId);
            }

            // --- Step 1: Upload File to Supabase Storage ---
            // Copy the file stream to a MemoryStream so it can be read multiple times (for upload and analysis)
            await using var fileMemoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(fileMemoryStream);
            fileMemoryStream.Position = 0; // Reset position for reading

            var sanitizedFileName = Path.GetFileName(file.FileName);
            var storagePath = $"{trailDto.Slug}/{trailId}/{sanitizedFileName}";

            var uploadSuccess = await storageService.UploadGpxFileAsync(
                trailId, // Or adapt storage service to just take storagePath
                trailDto.Slug,
                fileMemoryStream, // Pass MemoryStream for upload
                sanitizedFileName);

            if (!uploadSuccess)
            {
                _logger.LogError($"Failed to upload GPX file to Supabase for trail {trailId}.");
                return StatusCode(500, "Failed to upload GPX file to storage.");
            }

            // --- Step 2: AnalyzeGpxPointsBySurfaceType GPX File ---
            fileMemoryStream.Position = 0; // Reset stream position again for analysis
            var gpxAnalysisInfo = await gpxService.AnalyzeGpxTrack(fileMemoryStream);
            
            // --- Step 3: Dispatch Command to Process Analysis and Update DB ---
            var createdByUserId = GetUserIdFromClaims(); // Implement this helper method

            var processCommand = new ProcessGpxFileCommand(
                TrailId: trailId,
                StoragePath: storagePath,
                OriginalFileName: file.FileName,
                FileName: sanitizedFileName,
                FileSize: file.Length,
                ContentType: file.ContentType ?? "application/octet-stream",
                CreatedBy: createdByUserId, // This user is the 'CreatedBy' for GpxFile and 'UpdatedBy' for Trail

                // Analysis results
                AnalyzedDistance: gpxAnalysisInfo.Distance,
                AnalyzedElevationGain: gpxAnalysisInfo.ElevationGain,
                AnalyzedDifficultyLevel: gpxAnalysisInfo.DifficultyLevel,
                AnalyzedRouteType: gpxAnalysisInfo.RouteType,
                AnalyzedTerrainType: gpxAnalysisInfo.TerrainType,
                AnalyzedRouteGeom: gpxAnalysisInfo.RouteGeom
            );

            var gpxFileMetadataId = await mediator.Send(processCommand);

            return Ok(new { Message = "GPX file uploaded, analyzed, and trail updated successfully.", GpxFileMetadataId = gpxFileMetadataId });
        }
        catch (TrailNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (InvalidOperationException ex) // Catch errors from GpxService/AnalysisService
        {
            _logger.LogError(ex, "Error during GPX file analysis for trail {TrailId}.", trailId);
            return BadRequest(new ErrorResponse { Message = $"GPX file analysis failed: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("download")] // Route: api/trails/{trailId}/gpx-file/download
    public async Task<ActionResult> Download(Guid trailId)
    {
        try
        {
            // First, query the gpx_files metadata table to get the storage path and original filename
            // You'll need a new MediatR query for this (e.g., GetGpxFileMetadataQuery)
            var gpxMetadata = await mediator.Send(new GetGpxFileMetadataQuery(trailId));

            if (gpxMetadata == null)
            {
                _logger.LogWarning($"GPX file metadata not found for trail {trailId}.");
                return NotFound($"No GPX file associated with trail ID {trailId}.");
            }

            // Use the storage_path from metadata to download the actual file
            var (fileStream, downloadedFileName) = await storageService.DownloadGpxFileAsync(
                gpxMetadata.StoragePath); // Modify DownloadGpxFileAsync to take just storagePath

            if (fileStream == null)
            {
                _logger.LogWarning($"GPX file not found in storage at path {gpxMetadata.StoragePath}. Metadata exists but file missing.");
                // This could indicate a data inconsistency; might log as error.
                return NotFound($"GPX file content not found for trail ID {trailId}.");
            }

            Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{gpxMetadata.OriginalFileName}\"");
            return File(fileStream, gpxMetadata.ContentType, gpxMetadata.OriginalFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("metadata")] // Route: api/trails/{trailId}/gpx-file/metadata
    public async Task<ActionResult> GetMetadata(Guid trailId)
    {
        try
        {
            // First, query the gpx_files metadata table to get the storage path and original filename
            // You'll need a new MediatR query for this (e.g., GetGpxFileMetadataQuery)
            var gpxMetadata = await mediator.Send(new GetGpxFileMetadataQuery(trailId));

            if (gpxMetadata == null)
            {
                _logger.LogWarning($"GPX file metadata not found for trail {trailId}.");
                return NotFound($"No GPX file associated with trail ID {trailId}.");
            }

            return Ok(gpxMetadata);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    // A helper for getting the user ID (e.g., from HttpContext.User.Claims)
    private static Guid GetUserIdFromClaims()
    {
        // This is a placeholder. Implement proper JWT or authentication
        // claim extraction here.
        // Example: Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("User ID not found."));
        return Guid.Parse("00000000-0000-0000-0000-000000000001"); // Dummy ID for now
    }
}