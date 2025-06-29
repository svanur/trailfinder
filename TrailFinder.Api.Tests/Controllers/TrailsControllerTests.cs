using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Geometries;
using TrailFinder.Api.Controllers;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace TrailFinder.Api.Tests.Controllers;

public class TrailsControllerTests
{
    private readonly TrailsController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public TrailsControllerTests()
    {
        var loggerMock = new Mock<ILogger<TrailsController>>();
        _mediatorMock = new Mock<IMediator>();
        var storageServiceMock = new Mock<ISupabaseStorageService>();

        _controller = new TrailsController(
            _mediatorMock.Object, 
            loggerMock.Object,
            storageServiceMock.Object
        );
    }

    #region GetTrails
    
    [Fact]
    public async Task GetTrails_WhenSuccess_ReturnsTrails()
    {
        // Arrange
        var trails = new List<TrailDto>
        {
            CreateTrailDto("Trail 1"),
            CreateTrailDto("Trail 2")
        };

        var expectedTrails = new PaginatedResult<TrailDto>(
            trails, 2, 1, 1
            );
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrails);

        // Act
        var result = await _controller.GetTrails();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrails = Assert.IsType<PaginatedResult<TrailDto>>(okResult.Value, false);
        Assert.Equal(expectedTrails, returnedTrails);
        
        _mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetTrails_WhenNoTrailsFound_ReturnsEmptyCollection()
    {
        // Arrange
        var emptyResult = new PaginatedResult<TrailDto>(
            new List<TrailDto>(),
            0,
            1,
            10
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyResult);

        // Act
        var result = await _controller.GetTrails();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrails = Assert.IsType<PaginatedResult<TrailDto>>(okResult.Value);
        Assert.Empty(returnedTrails.Items);
        Assert.Equal(0, returnedTrails.TotalCount);
    }

    [Fact]
    public async Task GetTrails_WhenExceptionOccurs_ReturnsHandledException()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result = await _controller.GetTrails();

        // Assert
        // Note: The exact return type here depends on your HandleException implementation
        // Adjust this assertion based on how the HandleException method works
        Assert.NotNull(result.Result);
        // Might want to add more specific assertions based on your error handling logic
    }

    #endregion
    
    #region GetTrailBySlug
    
    [Fact]
    public async Task GetTrail_WithValidSlug_ReturnsOkResult()
    {
        // Arrange
        const string slug = "test-trail";
        var expectedTrail = CreateTrailDto("Trail 1", slug: slug);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrail);

        // Act
        var result = await _controller.GetTrail(slug);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrail = Assert.IsType<TrailDto>(okResult.Value);
        Assert.Equal(expectedTrail.Id, returnedTrail.Id);
        Assert.Equal(expectedTrail.Name, returnedTrail.Name);
        Assert.Equal(expectedTrail.Slug, returnedTrail.Slug);
    }

    [Fact]
    public async Task GetTrail_WithNonExistentSlug_ReturnsNotFound()
    {
        // Arrange
        var slug = "non-existent-trail";
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrailDto)null);

        // Act
        var result = await _controller.GetTrail(slug);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTrail_WhenExceptionOccurs_ReturnsHandledException()
    {
        // Arrange
        var slug = "error-trail";
        var expectedException = new Exception("Test exception");
        
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result = await _controller.GetTrail(slug);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
    }
    
    #endregion
    
    private static TrailDto CreateTrailDto(
        string name,
        string slug = "",
        string description = "",
        double distance = 0,
        double elevationGainMeters = 0,
        DifficultyLevel difficultyLevel = DifficultyLevel.Unknown,
        double startPointLatitude = 0,
        double startPointLongitude = 0,
        double endPointLatitude = 0,
        double endPointLongitude = 0,
        LineString? routeGeom = null,
        string? webUrl = "",
        bool hasGpx = false
    )
    {
        return new TrailDto(
            Guid.NewGuid(),
            name,
            slug,
            description,
            distance,
            elevationGainMeters,
            difficultyLevel,
            startPointLatitude,
            startPointLongitude,
            endPointLatitude,
            endPointLongitude,
            routeGeom,
            webUrl,
            hasGpx,
            DateTime.Now,
            DateTime.Now,
            Guid.NewGuid()
        );
    }
}