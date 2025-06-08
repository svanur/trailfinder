using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Entities;

namespace TrailFinder.Core.Interfaces.Repositories;

public interface ITrailRepository : IBaseRepository<Trail>
{
    
    Task<Trail?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    
    Task<PaginatedResult<Trail>> GetFilteredAsync(TrailFilterDto filter, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
}