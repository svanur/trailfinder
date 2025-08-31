// TrailFinder.Infrastructure/Persistence/Repositories/TrailRepository.cs

using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class TrailRepository(ApplicationDbContext context)
    : BaseRepository<Trail>(context), ITrailRepository
{
    public override async Task<IEnumerable<Trail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public IQueryable<Trail> GetTrailsSortedByDistanceToUser(
        double userLatitude,
        double userLongitude,
        CancellationToken cancellationToken = default
    )
    {
        // Define the user's point in the standard WGS 84 (SRID 4326)
        var userPoint = new Point(userLongitude, userLatitude)
        {
            SRID = 4326
        };

        // Use the DistanceMeters method directly on the geometry object.
        // EF Core will translate this into the correct ST_Distance(geometry, geometry) call.
        return Context.Trails
            .OrderBy(t => t.RouteGeom!.Distance(userPoint));
    }

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
            .Include(r => r.TrailLocations) // Eagerly load all TrailLocations for this Trail
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return trail;
    }
}