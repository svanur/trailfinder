using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Exceptions;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public class UpdateTrailCommandHandler : IRequestHandler<UpdateTrailCommand, Unit>
{
    private readonly ILogger<UpdateTrailCommandHandler> _logger;
    
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326); // SRID 4326 is for WGS84 (standard GPS coordinates)
    
    private readonly IApplicationDbContext _context;

    public UpdateTrailCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateTrailCommandHandler> logger
        )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateTrailCommand request, CancellationToken cancellationToken)
    {
         var trail = await _context.Set<Core.Entities.Trail>()
            .FirstOrDefaultAsync(t => t.Id == request.TrailId, cancellationToken);

        if (trail == null)
        {
            throw new TrailNotFoundException(request.TrailId);
        }

        // Add at the start of the Handle method:
        _logger.LogInformation($"Updating trail {request.TrailId} with " +
                               $"Difficulty={request.DifficultyLevel}" +
                               $"RouteType={request.RouteType}" +
                               $"TerrainType={request.TerrainType}"
        );

        // Handle nullable values
        /*if (request.Distance.HasValue)
        {
            trail.Distance = request.Distance.Value;
        }*/

        /*if (request.ElevationGain.HasValue)
        {
            trail.ElevationGain = request.ElevationGain.Value;
        }*/

        if (request.DifficultyLevel.HasValue)
        {
            trail.DifficultyLevel = request.DifficultyLevel.Value;
        }

        if (request.RouteType.HasValue)
        {
            trail.RouteType = request.RouteType.Value;
        }

        if (request.TerrainType.HasValue)
        {
            trail.TerrainType = request.TerrainType.Value;
        }

        // Update geometry points
        /*if (request.StartPoint.HasValue)
        {
            var startPoint = request.StartPoint.Value;
            trail.StartPoint = GeometryFactory.CreatePoint(
                new CoordinateZ(startPoint.Longitude, startPoint.Latitude, 0));
        }*/

        /*if (request.EndPoint.HasValue)
        {
            var endPoint = request.EndPoint.Value;
            trail.EndPoint = GeometryFactory.CreatePoint(
                new CoordinateZ(endPoint.Longitude, endPoint.Latitude, 0));
        }*/

        // For RouteGeom, we need to ensure it has Z coordinates as well
        if (request.RouteGeom != null)
        {
            // Create a new LineString with Z coordinates if the incoming one doesn't have them
            var coordinates = request.RouteGeom.Coordinates;
            var coordsWithZ = coordinates.Select(c => 
                c is CoordinateZ ? c : new CoordinateZ(c.X, c.Y, 0)).ToArray();
            
            trail.RouteGeom = GeometryFactory.CreateLineString(coordsWithZ);
        }


        trail.HasGpx = true;

        _context.Set<Core.Entities.Trail>().Update(trail);

        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            if (result <= 0)
            {
                throw new Exception("No changes were saved to the database.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update trail {request.TrailId}", ex);
        }

        // Add before returning:
        _logger.LogInformation($"Trail {request.TrailId} updated successfully");

        return Unit.Value;
    }
    
    /*public async Task<Unit> Handle(UpdateTrailCommand request, CancellationToken cancellationToken)
    {
        var trail = await _context.Set<Core.Entities.Trail>()
            .FirstOrDefaultAsync(t => t.Id == request.TrailId, cancellationToken);

        if (trail == null)
        {
            throw new TrailNotFoundException(request.TrailId);
        }

        // Handle nullable values
        if (request.Distance.HasValue)
        {
            trail.Distance = request.Distance.Value;
        }
    
        if (request.ElevationGain.HasValue)
        {
            trail.ElevationGain = request.ElevationGain.Value;
        }
    
        if (request.DifficultyLevel.HasValue)
        {
            trail.DifficultyLevel = request.DifficultyLevel.Value;
        }
        
        /*
        // Update points only if they are provided
        if (request.StartPoint.HasValue)
        {
            var startPoint = request.StartPoint.Value;
            trail.StartPoint = GeometryFactory.CreatePoint(
                new Coordinate(startPoint.Longitude, startPoint.Latitude));
        }

        if (request.EndPoint.HasValue)
        {
            var endPoint = request.EndPoint.Value;
            trail.EndPoint = GeometryFactory.CreatePoint(
                new Coordinate(endPoint.Longitude, endPoint.Latitude));
        }

        if (request.RouteGeom != null)
        {
            trail.RouteGeom = request.RouteGeom;
        }
        #1#

        trail.HasGpx = true;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new TrailNotFoundException(request.TrailId, ex);
        }

        return Unit.Value;
    }*/
}
