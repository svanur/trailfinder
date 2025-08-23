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

    // other methods...
    
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
    
}