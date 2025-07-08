// TrailFinder.Infrastructure/Persistence/Repositories/TrailRepository.cs

using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class RaceRepository : BaseRepository<Race>, IRaceRepository
{
    public RaceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Race>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderByDescending(r => r.CreatedAt) //TODO: order by recurring date
            .ToListAsync(cancellationToken);
    }

    public async Task<Race?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Slug == slug, cancellationToken);
    }

    public async Task<Race?> GetByIdWithLocationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Races // Your DbSet for Races
            .Include(r => r.RaceLocations) // Eagerly load all TrailLocations for this Trail
            .ThenInclude(rl => rl.Location) // Then, for each RaceLocation, eagerly load its associated Location
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}