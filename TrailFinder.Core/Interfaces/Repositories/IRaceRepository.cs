using TrailFinder.Core.Entities;

namespace TrailFinder.Core.Interfaces.Repositories;

public interface IRaceRepository : IBaseRepository<Race>
{
    Task<Race?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    // Add a new method to get a Race with its related locations eagerly loaded
    Task<Race?> GetByIdWithLocationsAsync(Guid id, CancellationToken cancellationToken = default);
}