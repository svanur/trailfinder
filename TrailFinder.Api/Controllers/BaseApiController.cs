using Microsoft.AspNetCore.Mvc;

namespace TrailFinder.Api.Controllers;

public class ErrorResponse
{
    public required string Message { get; set; }
    public string? Details { get; set; }
}

[ApiController]
public class BaseApiController : ControllerBase
{
    private readonly ILogger<BaseApiController> _logger;
    
    public BaseApiController(
        ILogger<BaseApiController> logger
    )
    {
        _logger = logger;
    }

    protected ActionResult HandleException(Exception ex)
    {
        _logger.LogError(ex, "An error occurred while processing the request");

        var response = new ErrorResponse
        {
            Message = "An unexpected error occurred",
            Details = ex.Message
        };

        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }
}
