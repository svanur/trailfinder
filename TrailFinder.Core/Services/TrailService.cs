// TrailFinder.Core/Services/TrailService.cs

using AutoMapper;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Core.Services;

public class TrailService : ITrailService
{
    private readonly ITrailRepository _trailRepository;

    public TrailService(ITrailRepository trailRepository)
    {
        _trailRepository = trailRepository;
    }

    public async Task<IEnumerable<Trail>> GetTrailsAsync()
    {
        return await _trailRepository.GetAllAsync();
    }

    public async Task<Trail?> GetTrailBySlugAsync(string slug)
    {
        return await _trailRepository.GetBySlugAsync(slug);
    }

    public async Task<Trail> CreateTrailAsync(Trail trail)
    {
        return await _trailRepository.CreateAsync(trail);
    }

    public async Task<Trail?> UpdateTrailAsync(string id, Trail trail)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            return null;
        }

        var existingTrail = await _trailRepository.GetByIdAsync(guidId);
        return existingTrail != null 
            ? await _trailRepository.UpdateAsync(trail)
            : null;
    }

    public async Task<bool> DeleteTrailAsync(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            return false;
        }

        return await _trailRepository.DeleteAsync(guidId);
    }
}
