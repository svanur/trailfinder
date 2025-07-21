// TrailFinder.Infrastructure/Repositories/GpxFileRepository.cs
// (Assuming your infrastructure project handles database access)

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;
// For PaginatedResult
// Your DbContext location

namespace TrailFinder.Infrastructure.Persistence.Repositories;

public class GpxFileRepository : IGpxFileRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GpxFileRepository> _logger;

    public GpxFileRepository(ApplicationDbContext dbContext, ILogger<GpxFileRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<GpxFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Always filter by IsActive = true for common queries unless explicitly needed
        return await _dbContext.GpxFiles
            .AsNoTracking() // Use AsNoTracking for read-only operations for performance
            .FirstOrDefaultAsync(gf => gf.Id == id && gf.IsActive, cancellationToken);
    }

    public async Task<GpxFile?> GetByTrailIdAsync(Guid trailId, CancellationToken cancellationToken = default)
    {
        // Get by TrailId, ensuring it's active
        return await _dbContext.GpxFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(
                gf => gf.TrailId == trailId
                      && gf.IsActive, 
                cancellationToken
            );
    }

    public async Task<GpxFile?> GetByStoragePathAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        // Get by StoragePath, ensuring it's active
        return await _dbContext.GpxFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(gf => gf.StoragePath == storagePath && gf.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<GpxFile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = _dbContext.GpxFiles.Where(gf => gf.IsActive);
        
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .ToListAsync(cancellationToken);

        return new List<GpxFile>(items);
    }

    public async Task<GpxFile> CreateAsync(GpxFile entity, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new GPX file metadata record for trail {TrailId}", entity.TrailId);
        entity.CreatedAt = DateTime.UtcNow;
        entity.IsActive = true; // Ensure active on creation
        // entity.CreatedBy should be set by the command handler before passing to repository

        _dbContext.GpxFiles.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("GPX file metadata record created with ID: {GpxFileId}", entity.Id);
        return entity;
    }

    public async Task<GpxFile?> UpdateAsync(GpxFile entity, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating GPX file metadata record with ID: {GpxFileId}", entity.Id);

        // Attach the entity to the context in Modified state, or fetch and update
        // The trigger will handle updated_at. Application sets updated_by.
        _dbContext.GpxFiles.Update(entity);
        // If entity.UpdatedAt and entity.UpdatedBy are being set by your app, do so here:
        entity.UpdatedAt = DateTime.UtcNow; // This will be overwritten by the trigger, but is good for consistency.
        // entity.UpdatedBy should be set by the command handler before passing to repository

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("GPX file metadata record ID: {GpxFileId} updated successfully.", entity.Id);
            return entity;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict when updating GPX file metadata record ID: {GpxFileId}.",
                entity.Id);
            return null; // Or throw a specific concurrency exception
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating GPX file metadata record ID: {GpxFileId}.", entity.Id);
            throw; // Re-throw general exceptions
        }
    }

    // This method performs a SOFT DELETE
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // This base method should be overridden or implemented to perform a SOFT delete
        // if your application uses soft deletes.
        _logger.LogInformation("Attempting to soft delete GPX file metadata record with ID: {GpxFileId}", id);

        var gpxFile = await _dbContext.GpxFiles.FirstOrDefaultAsync(gf => gf.Id == id, cancellationToken);
        if (gpxFile == null)
        {
            _logger.LogWarning("GPX file metadata record with ID: {GpxFileId} not found for soft delete.", id);
            return false;
        }

        gpxFile.IsActive = false; // Perform the soft delete
        // If your BaseEntity has UpdatedBy/UpdatedAt, you'd set them here too
        // gpxFile.UpdatedAt = DateTime.UtcNow;
        // gpxFile.UpdatedBy = <user_id_performing_delete>; // You'd need this passed in

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("GPX file metadata record with ID: {GpxFileId} soft deleted successfully.", id);
        return true;
    }

    // Specific method for soft deletion if you want to pass updatedBy
    public async Task<bool> SoftDeleteAsync(Guid id, Guid updatedBy, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Attempting to soft delete GPX file metadata record with ID: {GpxFileId} by user {UpdatedBy}", id,
            updatedBy);

        var gpxFile = await _dbContext.GpxFiles.FirstOrDefaultAsync(gf => gf.Id == id, cancellationToken);
        if (gpxFile == null)
        {
            _logger.LogWarning("GPX file metadata record with ID: {GpxFileId} not found for soft delete.", id);
            return false;
        }

        gpxFile.IsActive = false;
        gpxFile.UpdatedBy = updatedBy;
        // The trigger will set UpdatedAt

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation(
            "GPX file metadata record with ID: {GpxFileId} soft deleted successfully by user {UpdatedBy}.", id,
            updatedBy);
        return true;
    }
}