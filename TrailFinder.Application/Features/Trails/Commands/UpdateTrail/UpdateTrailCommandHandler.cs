using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Exceptions;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public class UpdateTrailCommandHandler : IRequestHandler<UpdateTrailCommand, Unit>
{
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326); // SRID 4326 is for WGS84 (standard GPS coordinates)
    
    private readonly IApplicationDbContext _context;

    public UpdateTrailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTrailCommand request, CancellationToken cancellationToken)
    {
        var trail = await _context.Set<Core.Entities.Trail>()
            .FirstOrDefaultAsync(t => t.Id == request.TrailId, cancellationToken);

        if (trail == null)
        {
            throw new TrailNotFoundException(request.TrailId);
        }

        // Handle nullable values
        if (request.DistanceMeters.HasValue)
        {
            trail.DistanceMeters = request.DistanceMeters.Value;
        }
    
        if (request.ElevationGainMeters.HasValue)
        {
            trail.ElevationGainMeters = request.ElevationGainMeters.Value;
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
        */

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
    }
}
