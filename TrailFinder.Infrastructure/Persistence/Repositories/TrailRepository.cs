// TrailFinder.Infrastructure/Persistence/Repositories/TrailRepository.cs
using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Requests;
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

    //TODO: ath með PaginatedResult
    public async Task<PaginatedResult<Trail>> GetFilteredAsync(
        TrailFilterDto filter, 
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.ToLower();
            query = query.Where(t => 
                t.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                t.Description.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));
        }

        if (filter.MinDistance.HasValue)
        {
            query = query.Where(t => t.DistanceMeters >= filter.MinDistance.Value);
        }
        
        if (filter.MaxDistance.HasValue)
        {
            query = query.Where(t => t.DistanceMeters <= filter.MaxDistance.Value);
        }

        if (filter.MinElevation.HasValue)
        {
            query = query.Where(t => t.ElevationGainMeters >= filter.MinElevation.Value);
        }

        if (filter.MaxElevation.HasValue)
        {
            query = query.Where(t => t.ElevationGainMeters <= filter.MaxElevation.Value);
        }

        if (filter.DifficultyLevel.HasValue)
        {
            query = query.Where(t => t.DifficultyLevel == filter.DifficultyLevel.Value);
        }

        // Get a total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "name" => filter.Descending 
                ? query.OrderByDescending(t => t.Name)
                : query.OrderBy(t => t.Name),
            "distance" => filter.Descending
                ? query.OrderByDescending(t => t.DistanceMeters)
                : query.OrderBy(t => t.DistanceMeters),
            "elevation" => filter.Descending
                ? query.OrderByDescending(t => t.ElevationGainMeters)
                : query.OrderBy(t => t.ElevationGainMeters),
            "difficulty" => filter.Descending
                ? query.OrderByDescending(t => t.DifficultyLevel)
                : query.OrderBy(t => t.DifficultyLevel),
            "created" => filter.Descending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        // Apply pagination
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Trail>(
            items,
            totalCount,
            filter.PageNumber,
            filter.PageSize
        );
    }

    public async Task<Trail?> GetByIdWithLocationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trail = await Context.Trails // Your DbSet for Trails
            .Include(r => r.TrailLocations) // Eagerly load all TrailLocations for this Trail
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return trail;
    }
}