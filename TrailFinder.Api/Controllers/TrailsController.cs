using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Commands.CreateTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TrailsController> _logger;

    public TrailsController(
        IMediator mediator,
        ILogger<TrailsController> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trail with slug {Slug}", slug);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTrailDto dto)
    {
        var command = CreateTrailCommand.FromDto(dto);
        var trailId = await _mediator.Send(command);
        return Ok(trailId);
    }
}
