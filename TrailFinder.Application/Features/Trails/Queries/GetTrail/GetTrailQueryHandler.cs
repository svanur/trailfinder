using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandler : IRequestHandler<GetTrailQuery, TrailDetailDto>
{
    private readonly ILogger<GetTrailQueryHandler> _logger;
    private readonly IMapper _mapper;

    private readonly ITrailRepository _trailRepository;
    private readonly GeometryFactory _geometryFactory;

    public GetTrailQueryHandler(
        ILogger<GetTrailQueryHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
        _geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    }

    public async Task<TrailDetailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        var trail = await _trailRepository.GetByIdWithLocationsAsync(request.Id, cancellationToken);
        if (trail == null)
        {
            throw new TrailNotFoundException(request.Id);
        }

        var trailDetailDto = _mapper.Map<TrailDetailDto>(trail);

        if (request is not { UserLatitude: not null, UserLongitude: not null })
        {
            return trailDetailDto;
        }

        var userLocationPoint = _geometryFactory.CreatePoint(
            new Coordinate(
                request.UserLongitude.Value,
                request.UserLatitude.Value
            )
        );
        
        // Calculate distance to user
        //trailDetailDto.DistanceToUserMeters = trail.RouteGeom?.Distance(userLocationPoint);
        //trailDetailDto.DistanceToUserKm = trailDetailDto.DistanceToUserMeters / 1000;

        return trailDetailDto;
    }
}
