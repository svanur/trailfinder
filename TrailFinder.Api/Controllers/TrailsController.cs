using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Commands.UpdateTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Core.DTOs.Gpx.Requests;
using TrailFinder.Core.DTOs.Gpx.Responses;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailsController : BaseApiController
{
    private readonly ILogger<TrailsController> _logger;
    private readonly IMediator _mediator;
    private readonly ISupabaseStorageService _storageService;

    public TrailsController(
        IMediator mediator,
        ILogger<TrailsController> logger,
        ISupabaseStorageService storageService
    )
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
        _storageService = storageService;
    }

    [HttpGet("{trailSlug}")]
    public async Task<ActionResult<TrailDto>> GetTrailBySlug(string trailSlug)
    {
        try
        {
            var result = await _mediator.Send(new GetTrailBySlugQuery(trailSlug));
            return result != null
                ? Ok(result)
                : NotFound();
        }
        /*
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        */
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    [HttpGet("{trailId:guid}")]
    public async Task<ActionResult<TrailDto?>> GetTrailById(Guid trailId)
    {
        try
        {
            var result = await _mediator.Send(new GetTrailQuery(trailId));
            return Ok(result);
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrailDto>>> GetAllTrails()
    {
        try
        {
            var paginatedResult = await _mediator.Send(new GetTrailsQuery());
            return Ok(paginatedResult);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    
    [HttpGet("{trailId:guid}/info")]
    public async Task<ActionResult<GpxInfoDto>> GetTrailGpxInfo(Guid trailId)
    {
        try
        {
            var result = await _mediator.Send(new GetTrailGpxInfoQuery(trailId));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    /*
    [HttpPut("{trailId:guid}/info")]
    public async Task<ActionResult> UpdateTrailGpxInfo(Guid trailId, UpdateGpxInfoDto gpxInfo)
    {
        try
        {
            var command = new UpdateTrailCommand(
                
                trailId,
                gpxInfo.Distance,
                gpxInfo.ElevationGain,
                gpxInfo.DifficultyLevel,
                gpxInfo.RouteType,
                gpxInfo.TerrainType,
                gpxInfo.RouteGeom
            );
        
            await _mediator.Send(command);
            return Ok();
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
    */

    [HttpPost("{trailId:guid}/gpx")]
    public async Task<ActionResult> UploadTrailGpx(Guid trailId, IFormFile file)
    {
        try
        {
            if (file.Length == 0)
            {
                return BadRequest("No file was uploaded");
            }

            if (!file.FileName.EndsWith(".gpx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("File must be a GPX file");
            }

            var trailResult = await GetTrailById(trailId);
            if (trailResult.Value == null)
            {
                throw new TrailNotFoundException(trailId);
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var success = await _storageService.UploadGpxFileAsync(
                trailId,
                trailResult.Value.Slug,
                stream, 
                file.FileName);

            if (!success)
            {
                return StatusCode(500, "Failed to upload GPX file");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    // [HttpPost]
    // public async Task<ActionResult<int>> Create(CreateTrailDto dto)
    // {
    //     var command = CreateTrailCommand.FromDto(dto);
    //     var trailId = await _mediator.Send(command);
    //     return Ok(trailId);
    // }
    
    /*
    // Example Controller Action (ASP.NET Core C#)
    [HttpPut("trails/{id}")] // Id is passed in the URL
    public async Task<IActionResult> UpdateTrail(Guid id, [FromBody] UpdateTrailDto updateDto)
    {
        // 1. Fetch the existing trail from the database using 'id'.
        // 2. Apply the nullable properties from 'updateDto' to the existing trail entity.
        //    (e.g., if Name is not null in updateDto, update the trail's name)
        // 3. Set the 'UpdatedBy' field on the entity from updateDto.UpdatedBy.
        // 4. Set the 'UpdatedAt' field on the entity to DateTime.UtcNow.
        // 5. Save the updated entity to the database.
        // 6. Return appropriate response (e.g., NoContent, Ok with updated DTO).
    }
    */
}