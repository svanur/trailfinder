using Microsoft.AspNetCore.Mvc;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Extensions;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailsController : ControllerBase
{
    private readonly ITrailService _trailService;
    private readonly ILogger<TrailsController> _logger;

    public TrailsController(ITrailService trailService, ILogger<TrailsController> logger)
    {
        _trailService = trailService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrailDto>>> GetTrails()
    {
        try
        {
            var trails = await _trailService.GetTrailsAsync();
            return Ok(trails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trails");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<TrailDto>> GetTrail(string slug)
    {
        var trail = await _trailService.GetTrailBySlugAsync(slug);
        return trail != null 
            ?  Ok(trail.ToDto())
            : NotFound();
    }
}
