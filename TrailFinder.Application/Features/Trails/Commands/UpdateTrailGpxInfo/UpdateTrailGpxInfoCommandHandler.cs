using MediatR;
using TrailFinder.Core.Exceptions;
using TrailFinder.Contract.Persistence;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrailGpxInfo;

public class UpdateTrailGpxInfoCommandHandler : IRequestHandler<UpdateTrailGpxInfoCommand, Unit>
{
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326); // SRID 4326 is for WGS84 (standard GPS coordinates)
    
    private readonly IApplicationDbContext _context;

    public UpdateTrailGpxInfoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTrailGpxInfoCommand request, CancellationToken cancellationToken)
    {
        var trail = await _context.Set<Core.Entities.Trail>()
            .FirstOrDefaultAsync(t => t.Id == request.TrailId, cancellationToken);

        if (trail == null)
        {
            throw new TrailNotFoundException(request.TrailId);
        }
        
        trail.DistanceMeters = request.DistanceMeters;
        trail.ElevationGainMeters = request.ElevationGainMeters;
        trail.DifficultyLevel = request.DifficultyLevel;
        trail.RouteType = request.RouteType;
        trail.TerrainType = request.TerrainType;
        
        // Create Point geometry for the start and end points
        trail.StartPoint = GeometryFactory.CreatePoint(
            new Coordinate(request.StartPoint.Longitude, request.StartPoint.Latitude));

        trail.EndPoint = GeometryFactory.CreatePoint(
            new Coordinate(request.EndPoint.Longitude, request.EndPoint.Latitude));

        trail.RouteGeom = request.RouteGeom;

        trail.HasGpx = true; // Since we're updating GPX info
        trail.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
