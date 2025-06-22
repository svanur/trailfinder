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
        try 
        {
            var trail = await _context.Set<Core.Entities.Trail>()
                .FirstOrDefaultAsync(t => t.Id == request.TrailId, cancellationToken);

            if (trail == null)
            {
                throw new TrailNotFoundException(request.TrailId);
            }
    
            trail.DistanceMeters = request.DistanceMeters;
            trail.ElevationGainMeters = request.ElevationGainMeters;
    
            // Create 3D points including elevation
            trail.StartPoint = GeometryFactory.CreatePoint(
                new CoordinateZ(
                    request.StartPoint.Longitude, 
                    request.StartPoint.Latitude, 
                    request.StartPoint.Elevation ?? 0
                ));

            trail.EndPoint = GeometryFactory.CreatePoint(
                new CoordinateZ(
                    request.EndPoint.Longitude, 
                    request.EndPoint.Latitude, 
                    request.EndPoint.Elevation ?? 0
                ));

            trail.RouteGeom = request.RouteGeom;
            trail.HasGpx = true;
            trail.UpdatedAt = DateTime.UtcNow;
    
            await _context.SaveChangesAsync(cancellationToken);
        
            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}
