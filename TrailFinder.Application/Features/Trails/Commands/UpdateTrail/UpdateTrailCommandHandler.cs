using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public class UpdateTrailCommandHandler : IRequestHandler<UpdateTrailCommand, Unit>
{
    private readonly ILogger<UpdateTrailCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    private readonly GeometryFactory _geometryFactory;
    
    public UpdateTrailCommandHandler(
        ITrailRepository trailRepository,
        GeometryFactory geometryFactory, 
        IMapper mapper,
        ILogger<UpdateTrailCommandHandler> logger
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
        _geometryFactory = geometryFactory; 
    }

    public async Task<Unit> Handle(UpdateTrailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            $"Attempting to update trail {request.TrailId} with data: {request}"); // Use request.TrailId for clarity

        var trailToUpdate = await _trailRepository.GetByIdAsync(request.TrailId, cancellationToken); // Use request.TrailId

        if (trailToUpdate == null)
        {
            _logger.LogWarning("Trail with ID {RequestTrailId} not found for update.", request.TrailId);
            throw new TrailNotFoundException(request.TrailId);
        }

        // Apply updates only if the request field is not null AND it's different from the current value
        // This avoids unnecessary database updates if no change occurred

        if (request.Name is not null &&
            !string.Equals(request.Name, trailToUpdate.Name,
                StringComparison.Ordinal)) // Or OrdinalIgnoreCase if name comparison is case-insensitive
        {
            trailToUpdate.Name = request.Name;
            _logger.LogInformation("Trail {RequestTrailId}: Name changed from '{Name}' to '{RequestName}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.Name, request.Name, request.UpdatedBy);
        }

        if (request.Description is not null &&
            !string.Equals(request.Description, trailToUpdate.Description, StringComparison.Ordinal))
        {
            trailToUpdate.Description = request.Description;
            _logger.LogInformation("Trail {RequestTrailId}: Name changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.Description, request.Description, request.UpdatedBy);
        }
        
        if (request.DistanceMeters.HasValue && !request.DistanceMeters.Value.Equals(trailToUpdate.DistanceMeters))
        {
            trailToUpdate.DistanceMeters = request.DistanceMeters.Value;
            _logger.LogInformation("Trail {RequestTrailId}: DistanceMeters changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.DistanceMeters, request.DistanceMeters, request.UpdatedBy);
        }

        if (request.ElevationGainMeters.HasValue && !request.ElevationGainMeters.Value.Equals((trailToUpdate.ElevationGainMeters)))
        {
            trailToUpdate.ElevationGainMeters = request.ElevationGainMeters.Value;
            _logger.LogInformation("Trail {RequestTrailId}: ElevationGainMeters changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.ElevationGainMeters, request.ElevationGainMeters, request.UpdatedBy);
        }

        if (request.ElevationLossMeters.HasValue && !request.ElevationLossMeters.Value.Equals((trailToUpdate.ElevationLossMeters)))
        {
            trailToUpdate.ElevationLossMeters = request.ElevationLossMeters.Value;
            _logger.LogInformation("Trail {RequestTrailId}: ElevationLossMeters changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.ElevationLossMeters, request.ElevationLossMeters, request.UpdatedBy);
        }

        if (request.DifficultyLevel.HasValue && request.DifficultyLevel.Value != trailToUpdate.DifficultyLevel)
        {
            trailToUpdate.DifficultyLevel = request.DifficultyLevel.Value;
            _logger.LogInformation("Trail {RequestTrailId}: DifficultyLevel changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.DifficultyLevel, request.DifficultyLevel, request.UpdatedBy);
        }

        if (request.RouteType.HasValue && request.RouteType.Value != trailToUpdate.RouteType)
        {
            trailToUpdate.RouteType = request.RouteType.Value;
            _logger.LogInformation("Trail {RequestTrailId}: RouteType changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.RouteType, request.RouteType, request.UpdatedBy);
        }

        if (request.TerrainType.HasValue && request.TerrainType.Value != trailToUpdate.TerrainType)
        {
            trailToUpdate.TerrainType = request.TerrainType.Value;
            _logger.LogInformation("Trail {RequestTrailId}: TerrainType changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.TerrainType, request.TerrainType, request.UpdatedBy);
        }

        if (request.SurfaceType.HasValue && request.SurfaceType.Value != trailToUpdate.SurfaceType)
        {
            trailToUpdate.SurfaceType = request.SurfaceType.Value;
            _logger.LogInformation("Trail {RequestTrailId}: SurfaceType changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.SurfaceType, request.SurfaceType, request.UpdatedBy);
        }

        if (request.isActive != trailToUpdate.IsActive)
        {
            trailToUpdate.IsActive = request.isActive;
            _logger.LogInformation("Trail {RequestTrailId}: IsActive changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.IsActive, request.isActive, request.UpdatedBy);
        }

        // Set audit fields
        trailToUpdate.UpdatedBy = request.UpdatedBy;
        trailToUpdate.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _trailRepository.UpdateAsync(trailToUpdate, cancellationToken);
            // If UpdateAsync always throws on failure, no need for the `if (updatedTrail == null)` check here.
            // The return value of UpdateAsync is not being used, so you can just await it.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to update trail {request.TrailId}."); // Log the exception details
            throw new Exception($"Failed to update trail {request.TrailId}", ex); // Re-throw with context
        }

        _logger.LogInformation($"Trail {request.TrailId} updated successfully: {trailToUpdate}.");

        return Unit.Value;
    }
}