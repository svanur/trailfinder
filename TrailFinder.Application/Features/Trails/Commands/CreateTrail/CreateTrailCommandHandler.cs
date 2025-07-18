// TrailFinder.Application/Features/Trails/Commands/CreateTrail/CreateTrailCommandHandler.cs
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using TrailFinder.Core.Entities;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Interfaces.Repositories;
namespace TrailFinder.Application.Features.Trails.Commands.CreateTrail;

public class CreateTrailCommandHandler : IRequestHandler<CreateTrailCommand, Guid>
{
    private readonly ILogger<CreateTrailCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    private readonly GeometryFactory _geometryFactory; // Inject GeometryFactory

    public CreateTrailCommandHandler(
        ILogger<CreateTrailCommandHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository,
        GeometryFactory geometryFactory) // Inject GeometryFactory
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
        _geometryFactory = geometryFactory;
    }

    public async Task<Guid> Handle(CreateTrailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Attempting to create a new trail with data: {request}");

        // Map the command DTO to your Trail entity
        // AutoMapper will handle most of the property copying.
        // Make sure your AutoMapper profile is set up to map CreateTrailCommand to Trail.
        var trailToCreate = _mapper.Map<Trail>(request);

        // Set audit fields that are handled by the server, not the client
        trailToCreate.CreatedAt = DateTime.UtcNow; // Set creation timestamp
        // trailToCreate.CreatedBy = request.CreatedBy; // This is already mapped by AutoMapper if property names match

        // Handle RouteGeom specifically if it needs transformation (e.g., ensuring Z coordinate)
        if (request.RouteGeom != null)
        {
            var coordinates = request.RouteGeom.Coordinates;
            var coordsWithZ = coordinates.Select(c =>
                c is CoordinateZ ? c : new CoordinateZ(c.X, c.Y, 0)).ToArray();

            trailToCreate.RouteGeom = _geometryFactory.CreateLineString(coordsWithZ);
        }
        else
        {
            // If RouteGeom is optional and not provided, ensure it's null or a default empty LineString
            trailToCreate.RouteGeom = null; // Or _geometryFactory.CreateLineString(new Coordinate[] {}); if you prefer empty geometry
        }

        Guid newTrailId;
        try
        {
            // Add the new trail to the repository
            var createdTrail = await _trailRepository.CreateAsync(trailToCreate, cancellationToken);
            newTrailId = createdTrail.Id; // Assuming AddAsync returns the entity with the new ID
            _logger.LogInformation($"Trail created successfully with ID: {newTrailId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create new trail.");
            throw new Exception("Failed to create new trail.", ex); // Re-throw with context
        }

        return newTrailId; // Return the ID of the newly created trail
    }
}