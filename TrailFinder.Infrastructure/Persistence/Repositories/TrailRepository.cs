// TrailFinder.Infrastructure/Persistence/Repositories/TrailRepository.cs

using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class TrailRepository(ApplicationDbContext context)
    : BaseRepository<Trail>(context), ITrailRepository
{
    public async Task<Trail?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<Trail?> GetByIdWithLocationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trail = await Context.Trails // Your DbSet for Trails
            .Include(t => t.TrailLocations) // Eagerly load all TrailLocations for this Trail
            .ThenInclude(tl => tl.Location) // Then, for each TrailLocation, eagerly load its associated Location
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return trail;
    }


    public override async Task<IEnumerable<Trail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}