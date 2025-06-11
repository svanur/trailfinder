using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailsController : BaseApiController
{
    private readonly ILogger<TrailsController> _logger;
    private readonly IMediator _mediator;

    public TrailsController(
        IMediator mediator,
        ILogger<TrailsController> logger
    )
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet("{slug}")]
    public async Task<ActionResult<TrailDto>> GetTrail(string slug)
    {
        try
        {
            var result = await _mediator.Send(new GetTrailBySlugQuery(slug));
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
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrailDto>> GetTrail(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetTrailQuery(id));
            return Ok(result);
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

    // [HttpPost]
    // public async Task<ActionResult<int>> Create(CreateTrailDto dto)
    // {
    //     var command = CreateTrailCommand.FromDto(dto);
    //     var trailId = await _mediator.Send(command);
    //     return Ok(trailId);
    // }
}