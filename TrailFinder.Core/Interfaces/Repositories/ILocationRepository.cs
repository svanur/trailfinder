using TrailFinder.Core.Entities;

namespace TrailFinder.Core.Interfaces.Repositories;

public interface ILocationRepository : IBaseRepository<Location>
{
    Task<Location?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}