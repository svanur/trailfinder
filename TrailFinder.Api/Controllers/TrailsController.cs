using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailsController : BaseApiController
{
    private readonly ILogger<TrailsController> _logger;
    private readonly IMediator _mediator;
    private readonly IGpxStorageService _gpxStorageService;

    public TrailsController(
        IMediator mediator,
        ILogger<TrailsController> logger,
        IGpxStorageService gpxStorageService
    )
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
        _gpxStorageService = gpxStorageService;
    }

    [HttpGet("{trailSlug}")]
    public async Task<ActionResult<TrailDto>> GetTrail(string trailSlug)
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
    public async Task<ActionResult<TrailDto?>> GetTrail(Guid trailId)
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
    public async Task<ActionResult<IEnumerable<TrailDto>>> GetTrails([FromQuery] Guid? parentId)
    {
        try
        {
            object? result;
            if (!parentId.HasValue)
            {
                result = await _mediator.Send(new GetTrailsQuery());
            }
            else
            {
                result = await _mediator.Send(new GetTrailsByParentIdQuery(parentId.Value));
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    
    [HttpGet("{trailId:guid}/gpx")]
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
            
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var success = await _gpxStorageService.UploadGpxFileAsync(
                trailId,
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
}