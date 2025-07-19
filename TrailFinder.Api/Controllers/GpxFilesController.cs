using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Application.Features.GpxFiles.Commands.CreateGpxFileMetadata;
using TrailFinder.Application.Features.GpxFiles.Queries.GetGpxFileMetadata; // New command

namespace TrailFinder.Api.Controllers;

/// <summary>
/// Controller responsible for managing GPX files associated with trails.
/// Provides endpoints to upload, download, and retrieve metadata for GPX files.
/// </summary>
[ApiController]
[Route("api/trails/{trailId:guid}/gpx-files")] // Updated route to be more precise
public class GpxFilesController(
    ILogger<BaseApiController> logger,
    IMediator mediator,
    ISupabaseStorageService storageService
) : BaseApiController(logger)
{
    private readonly IMediator _mediator = mediator;
    private readonly ISupabaseStorageService _storageService = storageService;

    [HttpPost("upload")] // Route: api/trails/{trailId}/gpx-files/upload
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

            // Get Trail details (including slug) from the MediatR query
            // Change to GetTrailDetailsQuery or similar if GetTrailQuery returns just the entity.
            var trail = await _mediator.Send(new GetTrailQuery(trailId));

            if (trail == null)
            {
                throw new TrailNotFoundException(trailId);
            }

            await using var stream = file.OpenReadStream();

            // Construct the storage path using your defined schema's logic
            // Assuming storage_path should be: {trail.Slug}/{trailId}/{file.FileName}
            // And file_name should be: file.FileName
            var sanitizedFileName = Path.GetFileName(file.FileName); // Just in case, ensure no pathing in filename
            var storagePath = $"{trail.Slug}/{trailId}/{sanitizedFileName}";

            var uploadSuccess = await _storageService.UploadGpxFileAsync(
                trailId, // Or perhaps just the storagePath?
                trail.Slug, // This parameter might become redundant for upload if storagePath handles it
                stream,
                sanitizedFileName); // Pass the final file name for storage service to potentially use

            if (!uploadSuccess)
            {
                _logger.LogError($"Failed to upload GPX file to Supabase for trail {trailId}.");
                return StatusCode(500, "Failed to upload GPX file to storage.");
            }

            // File uploaded successfully to Supabase Storage, now create the metadata record
            // You'll need to get the CreatedBy user ID, perhaps from claims or context
            var createdByUserId = GetUserIdFromClaims(); // Implement this helper method

            var createMetadataCommand = new CreateGpxFileMetadataCommand(
                TrailId: trailId,
                StoragePath: storagePath,
                OriginalFileName: file.FileName,
                FileName: sanitizedFileName, // Using the sanitized version for the 'file_name' column
                FileSize: file.Length,
                ContentType: file.ContentType ?? "application/octet-stream", // Use provided content type or fallback
                CreatedBy: createdByUserId
            );

            var gpxFileMetadataId = await _mediator.Send(createMetadataCommand);

            return Ok(new { Message = "GPX file uploaded and metadata created successfully.", GpxFileMetadataId = gpxFileMetadataId });
        }
        catch (TrailNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("download")] // Route: api/trails/{trailId}/gpx-files/download
    public async Task<ActionResult> Download(Guid trailId)
    {
        try
        {
            // First, query the gpx_files metadata table to get the storage path and original filename
            // You'll need a new MediatR query for this (e.g., GetGpxFileMetadataQuery)
            var gpxMetadata = await _mediator.Send(new GetGpxFileMetadataQuery(trailId));

            if (gpxMetadata == null)
            {
                _logger.LogWarning($"GPX file metadata not found for trail {trailId}.");
                return NotFound($"No GPX file associated with trail ID {trailId}.");
            }

            // Use the storage_path from metadata to download the actual file
            var (fileStream, downloadedFileName) = await _storageService.DownloadGpxFileAsync(
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

    [HttpGet("metadata")] // Route: api/trails/{trailId}/gpx-files/metadata
    public async Task<ActionResult> GetMetadata(Guid trailId)
    {
        try
        {
            // First, query the gpx_files metadata table to get the storage path and original filename
            // You'll need a new MediatR query for this (e.g., GetGpxFileMetadataQuery)
            var gpxMetadata = await _mediator.Send(new GetGpxFileMetadataQuery(trailId));

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