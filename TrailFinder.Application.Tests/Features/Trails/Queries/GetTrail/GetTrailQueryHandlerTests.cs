using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TrailFinder.Application.Features.Trails.Queries.GetTrail;
using TrailFinder.Common.TestsUtils;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Exceptions;
using Xunit;
using Assert = Xunit.Assert;

namespace TrailFinder.Application.Tests.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<DbSet<Core.Entities.Trail>> _trailDbSetMock;
    private readonly GetTrailQueryHandler _handler;

    public GetTrailQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _mapperMock = new Mock<IMapper>();
        _trailDbSetMock = new Mock<DbSet<Core.Entities.Trail>>();
        _handler = new GetTrailQueryHandler(_contextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ReturnsTrailDto()
    {
        // Arrange
        var trailId = Guid.NewGuid();
        //var trail = new Core.Entities.Trail { Id = trailId, Name = "Prófunarleið" };
        var trail = TrailUtils.CreateTrail("Prófunarleið");
        //var expectedDto = new TrailDto { Id = trailId, Name = "Prófunarleið" };
        var expectedDto = TrailUtils.CreateTrailDto ( "Prófunarleið" );
        var query = new GetTrailQuery(trailId);

        _contextMock
            .Setup(x => x.Set<Core.Entities.Trail>())
            .Returns(_trailDbSetMock.Object);

        _trailDbSetMock
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Core.Entities.Trail, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(trail);

        _mapperMock
            .Setup(x => x.Map<TrailDto>(trail))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result);
        _contextMock.Verify(x => x.Set<Core.Entities.Trail>(), Times.Once);
        _mapperMock.Verify(x => x.Map<TrailDto>(trail), Times.Once);
    }

    /*
    [Fact]
    public async Task Handle_WithInvalidId_ThrowsTrailNotFoundException()
    {
        // Arrange
        var invalidId = 999;
        var query = new GetTrailQuery(invalidId);

        _contextMock
            .Setup(x => x.Set<Core.Entities.Trail>())
            .Returns(_trailDbSetMock.Object);

        _trailDbSetMock
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Core.Entities.Trail, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Core.Entities.Trail?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<TrailNotFoundException>(() => 
            _handler.Handle(query, CancellationToken.None));

        Assert.Equal(invalidId, exception.Id);
        _contextMock.Verify(x => x.Set<Core.Entities.Trail>(), Times.Once);
        _mapperMock.Verify(x => x.Map<TrailDto>(It.IsAny<Core.Entities.Trail>()), Times.Never);
    }
    */
}
