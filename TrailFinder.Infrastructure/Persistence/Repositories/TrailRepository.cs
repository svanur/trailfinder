// TrailFinder.Infrastructure/Persistence/Repositories/TrailRepository.cs
using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Requests;
using TrailFinder.Core.DTOs.Trails.Responses;
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
            query = query.Where(t => t.Distance >= filter.MinDistance.Value);
        }
        
        if (filter.MaxDistance.HasValue)
        {
            query = query.Where(t => t.Distance <= filter.MaxDistance.Value);
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

        if (filter.RouteType.HasValue)
        {
            query = query.Where(t => t.RouteType == filter.RouteType.Value);
        }

        if (filter.TerrainType.HasValue)
        {
            query = query.Where(t => t.TerrainType == filter.TerrainType.Value);
        }
        
        //TODO: Add location to filter

        // Get a total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "name" => filter.Descending 
                ? query.OrderByDescending(t => t.Name)
                : query.OrderBy(t => t.Name),
            "distance" => filter.Descending
                ? query.OrderByDescending(t => t.Distance)
                : query.OrderBy(t => t.Distance),
            "elevation" => filter.Descending
                ? query.OrderByDescending(t => t.ElevationGainMeters)
                : query.OrderBy(t => t.ElevationGainMeters),
            "difficulty" => filter.Descending
                ? query.OrderByDescending(t => t.DifficultyLevel)
                : query.OrderBy(t => t.DifficultyLevel),
            "route_type" => filter.Descending
                ? query.OrderByDescending(t => t.RouteType)
                : query.OrderBy(t => t.RouteType),
            "terrain_type" => filter.Descending
                ? query.OrderByDescending(t => t.TerrainType)
                : query.OrderBy(t => t.TerrainType),
            // location
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

    public Task<Trail?> GetByIdWithLocationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    /*
    public override async Task<IEnumerable<Trail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => GetNewTrail(t))
            .ToListAsync(cancellationToken);
    
    }

    public override async Task<Trail?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(trail => trail.Id == id) 
            .Select(trail => GetNewTrail(trail)) 
            .FirstOrDefaultAsync(cancellationToken); // Get the firs°t (or default if not found)
    }

    private static Trail GetNewTrail(Trail trail)
    {   //TODO: hmm, method gets a Trail parameter and returns Trail hmm...
        return new Trail
        (
            trail.Id,
            trail.Name,
            trail.Description,
            
            0, //TODO: Placeholder
            0.0, //TODO: Placeholder
            
            trail.DifficultyLevel,
            trail.RouteType,
            trail.TerrainType,
            
            trail.RouteGeom,
            trail.UserId
        );
    }
    */
}