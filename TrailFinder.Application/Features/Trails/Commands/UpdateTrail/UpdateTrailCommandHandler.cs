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
            _logger.LogWarning($"Trail with ID {request.TrailId} not found for update.");
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
        
        if (request.Distance.HasValue && request.Distance.Value != trailToUpdate.Distance)
        {
            trailToUpdate.Distance = request.Distance.Value;
            _logger.LogInformation("Trail {RequestTrailId}: Distance changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.Distance, request.Distance, request.UpdatedBy);
        }

        if (request.ElevationGain.HasValue && request.ElevationGain.Value != trailToUpdate.ElevationGain)
        {
            trailToUpdate.ElevationGain = request.ElevationGain.Value;
            _logger.LogInformation("Trail {RequestTrailId}: ElevationGain changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.ElevationGain, request.ElevationGain, request.UpdatedBy);
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

        // For RouteGeom, we need to ensure it has Z coordinates as well
        if (request.RouteGeom != null)
        {
            // Create a new LineString with Z coordinates if the incoming one doesn't have them
            var coordinates = request.RouteGeom.Coordinates;
            var coordsWithZ = coordinates.Select(c =>
                c is CoordinateZ ? c : new CoordinateZ(c.X, c.Y, 0)).ToArray();

            trailToUpdate.RouteGeom = _geometryFactory.CreateLineString(coordsWithZ);
            _logger.LogInformation("Trail {RequestTrailId}: RouteGeom changed by {RequestUpdatedBy}", 
                request.TrailId, request.UpdatedBy);
        }

        // If you want to allow clearing WebUrl by passing null:
        //trailToUpdate.WebUrl = request.WebUrl;
        // If null means "no change", then:
        if (request.WebUrl is not null &&
            !string.Equals(request.WebUrl, trailToUpdate.WebUrl,
                StringComparison.OrdinalIgnoreCase)) // Often URLs are case-insensitive for comparison
        {
            trailToUpdate.WebUrl = request.WebUrl;
            _logger.LogInformation("Trail {RequestTrailId}: WebUrl changed from '{OldValue}' to '{NewValue}' by {RequestUpdatedBy}", 
                request.TrailId, trailToUpdate.WebUrl, request.WebUrl, request.UpdatedBy);
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