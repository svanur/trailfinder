using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class TrailLocationRepository(ApplicationDbContext context)
    : BaseRepository<TrailLocation>(context), ITrailLocationRepository
{
    public async Task<IEnumerable<TrailLocation>> GetByTrailIdAsync(Guid trailId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where( t => t.TrailId == trailId)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync(cancellationToken);
    }
}