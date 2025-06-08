// TrailFinder.Core/Interfaces/Services/ISupabaseService.cs
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Core.Interfaces.Services;

public interface ISupabaseService
{
    Task<TrailDto?> GetTrailBySlugAsync(string slug, CancellationToken cancellationToken = default);
    
    // Add other methods as needed
}