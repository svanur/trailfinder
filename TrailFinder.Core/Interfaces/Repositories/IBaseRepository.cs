    // TrailFinder.Core/Interfaces/Repositories/IBaseRepository.cs

    using TrailFinder.Core.DTOs.Common;
    using TrailFinder.Core.Entities.Common;

    namespace TrailFinder.Core.Interfaces.Repositories;

    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        
        // Add a generic paginated method
        /*
        Task<PaginatedResult<TEntity>> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? sortBy = null,
            bool sortDescending = false,
            CancellationToken cancellationToken = default
         );
        */
        
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        
        Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
       
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }