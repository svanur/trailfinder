using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Commands.UpdateTrailGpxInfo;
using TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;
using TrailFinder.Core.DTOs.Gpx.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/gpx-info")]
public class GpxInfoController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ISupabaseStorageService _storageService;

    public GpxInfoController(ILogger<BaseApiController> logger, IMediator mediator, ISupabaseStorageService storageService) : base(logger)
    {
        _mediator = mediator;
        _storageService = storageService;
    }

    [HttpGet("{trailId}")] // Changed from "gpx-info/{guid}"
    public async Task<ActionResult<GpxInfoDto>> GetGpxInfo(Guid trailId)
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
    
    
    [HttpPut("{trailId:guid}")]
    public async Task<ActionResult> UpdateTrailGpxInfo(Guid trailId, GpxInfoDto gpxInfo)
    {
        try
        {
            var command = new UpdateTrailGpxInfoCommand(
                trailId,
                gpxInfo.DistanceMeters,
                gpxInfo.ElevationGainMeters,
                gpxInfo.DifficultyLevel,
                gpxInfo.RouteType,
                gpxInfo.TerrainType,
                gpxInfo.StartPoint,
                gpxInfo.EndPoint,
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
}
