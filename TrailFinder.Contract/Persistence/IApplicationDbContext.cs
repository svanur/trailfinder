using Microsoft.EntityFrameworkCore;

namespace TrailFinder.Contract.Persistence;

public interface IApplicationDbContext
{
    /// <summary>
    /// Provides a DbSet instance for accessing database entities of the specified type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type for which the DbSet is required.</typeparam>
    /// <returns>A DbSet of the specified entity type.</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}