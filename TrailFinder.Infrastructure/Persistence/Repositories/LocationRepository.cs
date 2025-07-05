using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class LocationRepository(ApplicationDbContext context)
    : BaseRepository<Location>(context), ILocationRepository
{
    public async Task<Location?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public override async Task<IEnumerable<Location>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}