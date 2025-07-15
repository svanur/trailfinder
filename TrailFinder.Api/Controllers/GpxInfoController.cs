using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Commands.UpdateTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;
using TrailFinder.Core.DTOs.Gpx.Responses;
using TrailFinder.Core.Exceptions;
namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/gpx-info")]
public class GpxInfoController : BaseApiController
{
    private readonly IMediator _mediator;

    public GpxInfoController(
        ILogger<BaseApiController> logger, 
        IMediator mediator
    ) : base(logger)
    {
        _mediator = mediator;
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
}