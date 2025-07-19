// TrailFinder.Core.Interfaces.Repositories/IGpxFileRepository.cs

using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TrailFinder.Core.Interfaces.Repositories;

public interface IGpxFileRepository : IBaseRepository<GpxFile>
{
    // Specific method to get a GPX file metadata by its associated TrailId
    // Useful because trail_id is unique in the gpx_files table
    Task<GpxFile?> GetByTrailIdAsync(Guid trailId, CancellationToken cancellationToken = default);

    // If you need to search by storage path (e.g., for consistency checks)
    Task<GpxFile?> GetByStoragePathAsync(string storagePath, CancellationToken cancellationToken = default);

    // You might also want a method for soft deletion that explicitly sets IsActive to false
    // If your IBaseRepository<TEntity> has a DeleteAsync(Guid id),
    // you'll need to decide if that performs a hard delete or if you override/hide it.
    // For soft deletes, you typically use an Update operation internally.
    Task<bool> SoftDeleteAsync(Guid id, Guid updatedBy, CancellationToken cancellationToken = default);
}