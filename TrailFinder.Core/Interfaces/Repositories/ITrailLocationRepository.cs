using TrailFinder.Core.Entities;

namespace TrailFinder.Core.Interfaces.Repositories;

public interface ITrailLocationRepository : IBaseRepository<TrailLocation>
{
    //Task<IEnumerable<TrailLocation>> GetByTrailIdAsync(Guid trailId, CancellationToken cancellationToken = default);
}