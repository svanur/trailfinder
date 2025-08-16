// TrailFinder.Application.Features.Trails.Queries.GetTrails.GetTrailsQueryHandler.cs (Update this file)

using System.Diagnostics;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

public class GetTrailsQueryHandler : IRequestHandler<GetTrailsQuery, List<TrailListItemDto>>
{
    private readonly ILogger<GetTrailsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    
    public GetTrailsQueryHandler(
        ILogger<GetTrailsQueryHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
    }

    public async Task<List<TrailListItemDto>> Handle(
        GetTrailsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Request: {request.UserLatitude}, {request.UserLongitude}");

        if (request is { UserLatitude: not null, UserLongitude: not null })
        {
            // Get the sorted IQueryable from the repository
            var trailsQuery = _trailRepository.GetTrailsSortedByDistanceToUser(
                request.UserLatitude.Value,
                request.UserLongitude.Value
            );

            // Execute the query on the database and get the list of Trail entities
            var trails = await trailsQuery.ToListAsync(cancellationToken);
            
            // Map the list of Trail entities to DTOs in the handler
            var trailDtos = _mapper.Map<List<TrailListItemDto>>(trails);
            
            // The distance is not automatically populated in the DTO, so we need to calculate it again here
            // It's still efficient because we're only calculating for a few items.
            var userPoint = new Point(request.UserLongitude.Value, request.UserLatitude.Value)
            {
                SRID = 4326
            };
            
            foreach (var dto in trailDtos)
            {
                var originalTrail = trails.First(t => t.Id == dto.Id);
                if (originalTrail.RouteGeom == null)
                {
                    dto.DistanceToUserMeters = null;
                    dto.DistanceToUserKm = null;
                    continue;
                }

                var distanceInDegrees = originalTrail.RouteGeom.Distance(userPoint);
                var distanceInMeters = distanceInDegrees * 111320 * Math.Cos(request.UserLatitude.Value * Math.PI / 180);

                dto.DistanceToUserMeters = distanceInMeters;
                dto.DistanceToUserKm = distanceInMeters / 1000;
            }

            return trailDtos;
        }
        else
        {
            // If no user location, get all trails and sort them by name as default
            var trails = await _trailRepository.GetAllAsync(cancellationToken);
            var trailDtos = _mapper.Map<List<TrailListItemDto>>(trails);
            
            foreach (var dto in trailDtos)
            {
                dto.DistanceToUserMeters = null;
                dto.DistanceToUserKm = null;
            }
            
            return trailDtos.OrderBy(t => t.Name).ToList();
        }
    }
}