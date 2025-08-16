using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Races.Queries.GetRace;
using TrailFinder.Application.Features.Races.Queries.GetRaceBySlug;
using TrailFinder.Application.Features.Races.Queries.GetRaces;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacesController : BaseApiController
{
    private new readonly ILogger<RacesController> _logger;
    private readonly IMediator _mediator;
    private readonly ISupabaseStorageService _storageService;

    public RacesController(
        IMediator mediator,
        ILogger<RacesController> logger,
        ISupabaseStorageService storageService
    )
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
        _storageService = storageService;
    }

    [HttpGet("{raceSlug}")]
    public async Task<ActionResult<RaceDto>> GetRaceBySlug(string raceSlug)
    {
        try
        {
            var result = await _mediator.Send(new GetRaceBySlugQuery(raceSlug));
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

    [HttpGet("{raceId:guid}")]
    public async Task<ActionResult<RaceDto?>> GetRaceById(Guid raceId)
    {
        try
        {
            var result = await _mediator.Send(new GetRaceQuery(raceId));
            return Ok(result);
        }
        catch (RaceNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RaceDto>>> GetRaces()
    {
        try
        {
            var paginatedResult = await _mediator.Send(new GetRacesQuery());
            return Ok(paginatedResult);
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