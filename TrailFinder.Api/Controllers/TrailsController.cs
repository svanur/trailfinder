using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrailFinder.Application.Features.Trails.Commands.UpdateTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Core.DTOs.Trails.Requests;
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

    [HttpPut("trails/{trailId:guid}")]
    public async Task<IActionResult> UpdateTrail(
        Guid trailId,
        [FromBody] UpdateTrailDto updateTrailDto
    )
    {
        try
        {
            // 1. Construct the command directly from the incoming parameters
            var command = new UpdateTrailCommand(
                trailId, // Pass the trailId from the route
                updateTrailDto.Name,
                updateTrailDto.Description,
                updateTrailDto.Distance,
                updateTrailDto.ElevationGain,
                updateTrailDto.DifficultyLevel,
                updateTrailDto.RouteType,
                updateTrailDto.TerrainType,
                updateTrailDto.SurfaceType,
                updateTrailDto.RouteGeom,
                updateTrailDto.WebUrl,
                updateTrailDto.UpdatedBy
            );

            // 2. Send the command to MediatR.
            //    The MediatR pipeline (including the handler and FluentValidation)
            //    will handle all the business logic and persistence.
            await _mediator.Send(command);

            // 3. Return an appropriate HTTP response.
            //    204 No Content is often used for successful PUT/updates that don't return new data.
            //    200 OK is also fine if you prefer.
            return NoContent(); // Or return Ok(); if you prefer
        }
        catch (TrailNotFoundException ex)
        {
            // Catch specific custom exceptions and map them to appropriate HTTP responses.
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (ValidationException ex) // Catches validation errors from the pipeline
        {
            // If you have a custom validation error handling middleware, it might catch this.
            // Otherwise, you can handle it explicitly here.
            return BadRequest(new ErrorResponse
            {
                Message = "Validation failed",
                Details = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToString()
            });
        }
        catch (Exception ex)
        {
            // Catch any other unexpected exceptions.
            // HandleException() should log the error and return a generic 500 Internal Server Error.
            return HandleException(ex);
        }
    }
}