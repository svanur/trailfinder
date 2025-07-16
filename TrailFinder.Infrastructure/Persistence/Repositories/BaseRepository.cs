// TrailFinder.Infrastructure/Persistence/Repositories/BaseRepository.cs

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Interfaces.Repositories;
// Make sure this is present and correct:

// <--- This is essential for the string-based OrderBy/OrderByDescending


namespace TrailFinder.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<TEntity>(ApplicationDbContext context)
    : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TEntity? baseEntity;

        try
        {
            baseEntity = await DbSet.FindAsync([id], cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: remove or log
            throw;
        }

        return baseEntity;
    }

    public virtual async Task<PaginatedResult<TEntity>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        // Start with the base queryable
        IQueryable<TEntity> query = DbSet;

        // Apply sorting using System.Linq.Dynamic.Core
        var orderByClause = sortBy;
        if (!string.IsNullOrWhiteSpace(orderByClause))
        {
            if (sortDescending) orderByClause += " descending";
            // Correct way to call the extension method
            query = query.OrderBy(orderByClause);
        }
        else
        {
            // Default sorting if no sortBy is provided
            if (typeof(TEntity).GetProperty("CreatedAt") != null)
                // Correct way to call the extension method for OrderByDescending
                query = query.OrderByDescending(entity => "CreatedAt");
            else if (typeof(TEntity).GetProperty("Id") != null)
                // Correct way to call the extension method for OrderByDescending
                query = query.OrderByDescending(entity => "Id");
            // Fallback: If no default sortable property is found,
            // you might need to enforce sortBy or provide a default.
            // For stable pagination, an OrderBy clause is generally
            // recommended before Skip/Take.
        }

        // Get total count BEFORE applying pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<TEntity>(items, totalCount, pageNumber, pageSize);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        DbSet.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }
}