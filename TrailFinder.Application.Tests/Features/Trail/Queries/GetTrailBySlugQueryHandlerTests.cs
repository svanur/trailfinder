using AutoMapper;
using Moq;
using TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;
using TrailFinder.Common.TestsUtils;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Interfaces.Repositories;
using Xunit;
using Assert = Xunit.Assert;


namespace TrailFinder.Application.Tests.Features.Trail.Queries;

public class GetTrailBySlugQueryHandlerTests
{
    private readonly Mock<ITrailRepository> _trailRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetTrailBySlugQueryHandler _handler;

    public GetTrailBySlugQueryHandlerTests()
    {
        _trailRepositoryMock = new Mock<ITrailRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetTrailBySlugQueryHandler(_trailRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidSlug_ReturnsTrailDto()
    {
        // Arrange
        const string slug = "test-trail";
        var trail = TrailUtils.CreateTrail("Test trail"); 
        var expectedDto = TrailUtils.CreateTrailDto("Test trail", slug: "test-trail"); 
        var query = new GetTrailBySlugQuery(slug);

        _trailRepositoryMock
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trail);

        _mapperMock
            .Setup(x => x.Map<TrailDto>(trail))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result);
        _trailRepositoryMock.Verify(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<TrailDto>(trail), Times.Once);
    }

    /*
    [Fact]
    public async Task Handle_WithInvalidSlug_ThrowsTrailNotFoundException()
    {
        // Arrange
        const string slug = "nonexistent-trail";
        var query = new GetTrailBySlugQuery(slug);

        _trailRepositoryMock
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Trail?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TrailNotFoundException>(() => 
            _handler.Handle(query, CancellationToken.None));

        _trailRepositoryMock.Verify(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<TrailDto>(It.IsAny<Trail>()), Times.Never);
    }
    */
}
