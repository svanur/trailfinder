// TrailFinder.Infrastructure/Persistence/Repositories/RaceRepository.cs

using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class RaceRepository(ApplicationDbContext context) 
    : BaseRepository<Race>(context), IRaceRepository
{
    public override async Task<IEnumerable<Race>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // First, let's verify DbSet itself
        if (DbSet == null)
        {
            System.Diagnostics.Debug.WriteLine("DbSet<Race> is NULL!");
            // This would be very bad, indicating a DI or DbContext initialization problem
            throw new InvalidOperationException("Race DbSet is null in RaceRepository.");
        }

        IEnumerable<Race> resultList;
        try
        {
            // Put a breakpoint directly here
            resultList = await DbSet.ToListAsync(cancellationToken);
            System.Diagnostics.Debug.WriteLine($"RaceRepository: ToListAsync returned {resultList?.Count()} items.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RaceRepository: Exception during ToListAsync: {ex.Message}");
            Console.WriteLine(ex.ToString()); // Log full exception for more details
            throw; // Re-throw to see the exception properly
        }

        // Put a breakpoint here and inspect resultList
        return resultList;
    }


    public async Task<Race?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(r => r.Slug == slug, cancellationToken);
    }

    public async Task<Race?> GetByIdWithLocationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Races // Your DbSet for Races
            .Include(r => r.RaceLocations) // Eagerly load all TrailLocations for this Trail
            .ThenInclude(rl => rl.Location) // Then, for each RaceLocation, eagerly load its associated Location
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}