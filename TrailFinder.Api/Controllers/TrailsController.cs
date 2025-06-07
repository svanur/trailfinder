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

    /// <summary>
    /// Retrieves a list of all available trails.
    /// </summary>
    /// <returns>A collection of trails wrapped in an ActionResult.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trail>>> GetTrails()
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

    /// <summary>
    /// Retrieves details of a specific trail based on its unique slug.
    /// </summary>
    /// <param name="slug">The unique identifier (slug) of the trail to retrieve.</param>
    /// <returns>An ActionResult containing the trail details if found, otherwise a NotFound result.</returns>
    [HttpGet("{slug}")]
    public async Task<ActionResult<Trail>> GetTrail(string slug)
    {
        var trail = await _trailService.GetTrailBySlugAsync(slug);
        if (trail == null)
            return NotFound();

        return Ok(trail);
    }
}