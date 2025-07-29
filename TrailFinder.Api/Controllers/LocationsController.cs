using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Locations.GetLocation;
using TrailFinder.Application.Features.Locations.GetLocationBySlug;
using TrailFinder.Application.Features.Locations.GetLocations;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : BaseApiController
{
    private readonly ILogger<LocationsController> _logger;
    private readonly IMediator _mediator;
    private readonly ISupabaseStorageService _storageService;

    public LocationsController(
        IMediator mediator,
        ILogger<LocationsController> logger,
        ISupabaseStorageService storageService
    )
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
        _storageService = storageService;
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<TrailListItemDto>> GetLocationBySlug(string slug)
    {
        try
        {
            var result = await _mediator.Send(new GetLocationBySlugQuery(slug));
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
    public async Task<ActionResult<LocationDto?>> GetLocation(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetLocationQuery(id));
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
    public async Task<ActionResult<LocationDto>> GetLocations()
    {
        try
        {
            var paginatedResult = await _mediator.Send(new GetLocationsQuery());
            return Ok(paginatedResult);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}