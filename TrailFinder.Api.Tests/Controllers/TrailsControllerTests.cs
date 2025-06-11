using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrailFinder.Api.Controllers;
using TrailFinder.Application.Features.Trails.Queries.GetTrails;
using TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Enums;
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

        _controller = new TrailsController(
            _mediatorMock.Object, 
            loggerMock.Object
        );
    }

    [Fact]
    public async Task GetTrails_WithoutParentId_ReturnsAllTrails()
    {
        // Arrange
        var expectedTrails = new List<TrailDto>
        {
            CreateTrailDto("Trail 1"),
            CreateTrailDto("Trail 2")
        };

        var paginatedResult = new PaginatedResult<TrailDto>(
            expectedTrails,
            expectedTrails.Count,
            1, // pageNumber
            10 // pageSize
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _controller.GetTrails(null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrails = Assert.IsType<PaginatedResult<TrailDto>>(okResult.Value, exactMatch: false);
        Assert.Equal(expectedTrails, returnedTrails.Items);

        _mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTrailsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task GetTrails_WithParentId_ReturnsChildTrails()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var expectedTrails = new List<TrailDto>
        {
            CreateTrailDto("Child Trail 1", parentId),
            CreateTrailDto("Child Trail 2", parentId)
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailsByParentIdQuery>(q => q.ParentId == parentId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrails);

        // Act
        var result = await _controller.GetTrails(parentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTrails = Assert.IsType<IEnumerable<TrailDto>>(okResult.Value, false);
        Assert.Equal(expectedTrails, returnedTrails);


        _mediatorMock.Verify(
            x => x.Send(It.Is<GetTrailsByParentIdQuery>(q => q.ParentId == parentId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetTrails_WithParentId_WhenNoTrailsFound_ReturnsNotFound()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTrailsByParentIdQuery>(q => q.ParentId == parentId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<TrailDto>)null);

        // Act
        var result = await _controller.GetTrails(parentId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
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
        var result = await _controller.GetTrails(null);

        // Assert
        // Note: The exact return type here depends on your HandleException implementation
        // Adjust this assertion based on how your HandleException method works
        Assert.NotNull(result.Result);
        // You might want to add more specific assertions based on your error handling logic
    }

    private static TrailDto CreateTrailDto(
        string name,
        Guid? parentId = null,
        string slug = "",
        string description = "",
        decimal distanceMeters = 0,
        decimal elevationGainMeters = 0,
        TrailDifficulty difficultyLevel = TrailDifficulty.Moderate,
        double startPointLatitude = 0,
        double startPointLongitude = 0,
        string? webUrl = "",
        bool hasGpx = false
    )
    {
        return new TrailDto(
            Guid.NewGuid(),
            parentId,
            name,
            slug,
            description,
            distanceMeters,
            elevationGainMeters,
            difficultyLevel,
            startPointLatitude,
            startPointLongitude,
            webUrl,
            hasGpx,
            DateTime.Now,
            DateTime.Now,
            Guid.NewGuid().ToString()
        );
    }
}