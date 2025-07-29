using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Geometries;
using TrailFinder.Api.Controllers;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.GpxFile;
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

    private static TrailListItemDto NewTrailListItemDto(
        string name,
        
        string slug = "",
        string description = "",
        double distance = 0,
        double elevationGain = 0,
        
        DifficultyLevel difficultyLevel = DifficultyLevel.Unknown,
        RouteType routeType = RouteType.Unknown,
        TerrainType terrainType = TerrainType.Unknown,
        SurfaceType surfaceType = SurfaceType.Unknown,
         
        LineString? routeGeom = null
    )
    {
        return new TrailListItemDto(
            Guid.NewGuid(),
            name,
            slug,
            description,
            distance,
            elevationGain,
            difficultyLevel,
            routeType,
            terrainType,
            surfaceType,
            routeGeom,
            new GpxPoint(), //TODO ?
            new GpxPoint(),
            Guid.NewGuid(),
            DateTime.Now,
            null,
            null
        );
    }

    private static TrailDetailDto NewTrailDetailDto(
        string name,
        
        string slug = "",
        string description = "",
        double distance = 0,
        double elevationGain = 0,
        
        DifficultyLevel difficultyLevel = DifficultyLevel.Unknown,
        RouteType routeType = RouteType.Unknown,
        TerrainType terrainType = TerrainType.Unknown,
        SurfaceType surfaceType = SurfaceType.Unknown,
         
        LineString? routeGeom = null
    )
    {
        return new TrailDetailDto(
            //Guid.NewGuid(),
            name,
            slug,
            description,
            distance,
            elevationGain,
            difficultyLevel,
            routeType,
            terrainType,
            surfaceType,
            routeGeom,
            new GpxPoint(), //TODO ?
            new GpxPoint(),
            Guid.NewGuid(),
            DateTime.Now,
            null,
            null
        );
    }

    #region GetTrails

    [Fact]
    public async Task GetTrails_WhenSuccess_ReturnsTrails()
    {
        // Arrange
 

        var trails = new List<TrailListItemDto>
        {
            NewTrailListItemDto("Trail 1" ),
            NewTrailListItemDto("Trail 2" )
        };

        var expectedTrails = new List<TrailListItemDto>(
            trails
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrails);

        // Act
        var result = await _controller.GetAllTrails(null,null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrails = Assert.IsType<List<TrailListItemDto>>(okResult.Value, false);
        Assert.Equal(expectedTrails, returnedTrails);

        _mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetTrails_WhenNoTrailsFound_ReturnsEmptyCollection()
    {
        // Arrange
        var emptyResult = new List<TrailListItemDto>(
            new List<TrailListItemDto>()
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyResult);

        // Act
        var result = await _controller.GetAllTrails(null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrails = Assert.IsType<List<TrailListItemDto>>(okResult.Value);
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
        var result = await _controller.GetAllTrails(null, null);

        // Assert
        // Note: The exact return type here depends on your HandleException implementation
        // Adjust this assertion based on how the HandleException method works
        Assert.NotNull(result.Result);
        // Might want to add more specific assertions based on your error handling logic
        Assert.IsType<ObjectResult>(result.Result);
        
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<ErrorResponse>(objectResult.Value);
        
        var errorResponse = objectResult.Value as ErrorResponse;
        Assert.NotNull(errorResponse);
        Assert.Equal("Test exception", errorResponse.Details);
        Assert.Equal("An unexpected error occurred", errorResponse.Message);
    }

    #endregion

    #region GetTrailBySlug

    [Fact]
    public async Task GetTrail_WithValidSlug_ReturnsOkResult()
    {
        // Arrange
        const string slug = "test-trail";
 
        var expectedTrail = NewTrailDetailDto(
            "Trail 1" 
        );

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrail);

        // Act
        var result = await _controller.GetTrailBySlug(slug);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrail = Assert.IsType<TrailDetailDto>(okResult.Value);
        //Assert.Equal(expectedTrail.Id, returnedTrail.Id);
        Assert.Equal(expectedTrail.Name, returnedTrail.Name);
        Assert.Equal(expectedTrail.Slug, returnedTrail.Slug);
    }

    [Fact]
    public async Task GetTrail_WithNonExistentSlug_ReturnsNotFound()
    {
        // Arrange
        const string slug = "non-existent-trail";
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrailDetailDto)null!);

        // Act
        var result = await _controller.GetTrailBySlug(slug);

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
        var result = await _controller.GetTrailBySlug(slug);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
    }

    #endregion
}