using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using System.IO;
using System.Threading.Tasks;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]/{trailId:guid}")]
public class GpxFilesController(
    ILogger<BaseApiController> logger,
    IMediator mediator,
    ISupabaseStorageService storageService // Keep storage service here, or wrap in command
)
    : BaseApiController(logger) // Assuming BaseApiController handles common logging and exception mapping
{
    private readonly IMediator _mediator = mediator;
    private readonly ISupabaseStorageService _storageService = storageService;


    [HttpPost("upload")]
    [Consumes("multipart/form-data")] // Explicitly state the content type
    public async Task<ActionResult> Upload(Guid trailId, IFormFile file)
    {
        try
        {
            if (file.Length == 0)
            {
                return BadRequest("No file was uploaded or file is empty.");
            }

            // Basic file type validation (can be more robust with content type checks too)
            if (!file.FileName.EndsWith(".gpx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("File must be a GPX file.");
            }

            // Use MediatR to get the trail slug
            var trail = await _mediator.Send(new GetTrailQuery(trailId));

            if (trail == null)
            {
                // Let TrailNotFoundException be handled by your BaseApiController's HandleException or a middleware
                throw new TrailNotFoundException(trailId);
            }

            // Stream should be opened and kept alive for the upload operation
            // No need for MemoryStream if storageService can take IFormFile.OpenReadStream() directly
            await using var stream = file.OpenReadStream();

            var success = await _storageService.UploadGpxFileAsync(
                trailId,
                trail.Slug,
                stream,
                file.FileName);

            if (success)
            {
                return Ok("GPX file uploaded successfully.");
            }

            _logger.LogError($"Failed to upload GPX file for trail {trailId}. Storage service returned false.");
            return StatusCode(500, "Failed to upload GPX file due to an internal error.");

        }
        // Catch specific exceptions first if BaseApiController doesn't map them
        catch (TrailNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message }); // Assuming ErrorResponse is defined
        }
        catch (Exception ex)
        {
            // Delegate to BaseApiController's general exception handler
            return HandleException(ex);
        }
    }


    [HttpGet("download")] // Changed to HttpGet for downloads
    public async Task<ActionResult> Download(Guid trailId)
    {
        try
        {
            // Use MediatR to get the trail slug
            var trail = await _mediator.Send(new GetTrailQuery(trailId));

            if (trail == null)
            {
                throw new TrailNotFoundException(trailId);
            }

            var (fileStream, fileName) = await _storageService.DownloadGpxFileAsync(trailId, trail.Slug);

            if (fileStream == null)
            {
                _logger.LogWarning($"GPX file not found for trail {trailId} (slug: {trail.Slug}).");
                return NotFound($"GPX file not found for trail with ID {trailId}.");
            }

            // Set the content type and file name for the download
            Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            return File(fileStream, "application/gpx+xml"); // Standard GPX MIME type
            // Or "application/octet-stream" if you want a generic download
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
}