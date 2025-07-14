// TrailFinder.Infrastructure/Persistence/Repositories/TrailRepository.cs
using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Requests;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class TrailRepository : BaseRepository<Trail>, ITrailRepository
{
    public TrailRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Trail?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<PaginatedResult<Trail>> GetFilteredAsync(
        TrailFilterDto filter, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

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
            query = query.Where(t => t.ElevationGain >= filter.MinElevation.Value);
        }

        if (filter.MaxElevation.HasValue)
        {
            query = query.Where(t => t.ElevationGain <= filter.MaxElevation.Value);
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
                ? query.OrderByDescending(t => t.ElevationGain)
                : query.OrderBy(t => t.ElevationGain),
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

    public override async Task<IEnumerable<Trail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
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

            // Use NTS (NetTopologySuite) methods which map to PostGIS functions
            trail.RouteGeom
                .Length, // ST_Length in PostGIS, typically in SRID units (e.g., meters for 4326/geographic)

            // ElevationGain:
            // This is the trickiest. PostGIS has functions like ST_3DDistance,
            // but true "elevation gain" requires analyzing z-coordinates along the path,
            // which might not be directly available as a simple function.
            // You might need a custom DB function or client-side calculation for this.
            // For demonstration, let's assume a placeholder for ElevationGain:
            0.0, // Placeholder
            
            trail.DifficultyLevel,
            trail.RouteType,
            trail.TerrainType,
            trail.RouteGeom.StartPoint,
            trail.RouteGeom.EndPoint,
            trail.RouteGeom,
            trail.UserId
        );
    }
}