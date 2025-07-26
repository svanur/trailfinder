// TrailFinder.Application.Features.Trails.Queries.GetTrails.GetTrailsQueryHandler.cs (Update this file)
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

public class GetTrailsQueryHandler : IRequestHandler<GetTrailsQuery, List<TrailDto>>
{
    private readonly ILogger<GetTrailsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    private readonly GeometryFactory _geometryFactory;

    public GetTrailsQueryHandler(
        ILogger<GetTrailsQueryHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
        _geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    }

    public async Task<List<TrailDto>> Handle(
        GetTrailsQuery request,
        CancellationToken cancellationToken)
    {
        var trails = await _trailRepository.GetAllAsync(cancellationToken);
        var trailDtos = _mapper.Map<List<TrailDto>>(trails);
        
        _logger.LogInformation($"Request: ${request.UserLatitude}, ${request.UserLongitude}");

        if (request is { UserLatitude: not null, UserLongitude: not null })
        {
            var userLocationPoint = _geometryFactory.CreatePoint(
                new Coordinate(request.UserLongitude.Value, request.UserLatitude.Value)
            );

            foreach (var dto in trailDtos)
            {
                if (dto.RouteGeom == null || dto.RouteGeom.NumPoints <= 0)
                {
                    continue;
                }

                // Calculate distance to user
                dto.DistanceToUserMeters = dto.RouteGeom.Distance(userLocationPoint);
                dto.DistanceToUserKm = dto.DistanceToUserMeters / 1000;
            }

            // Sort the DTOs by distance to the user before returning
            // Null distances (e.g., if RouteGeom was null) will be sorted to the end
            trailDtos = trailDtos
                .OrderBy(t => t.DistanceToUserMeters.GetValueOrDefault(double.MaxValue))
                .ToList();
        }
        else
        {
            // If no user location, ensure DistanceToUserMeters/Km are null and sort by name as default
            foreach (var dto in trailDtos)
            {
                dto.DistanceToUserMeters = null;
                dto.DistanceToUserKm = null;
            }
            // If you want default sorting by name when no user location, do it here.
            trailDtos = trailDtos.OrderBy(t => t.Name).ToList();
        }

        return trailDtos;
    }
}
