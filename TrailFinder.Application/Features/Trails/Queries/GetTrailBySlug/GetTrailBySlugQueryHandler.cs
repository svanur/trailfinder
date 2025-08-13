using AutoMapper;
using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;

public class GetTrailBySlugQueryHandler : IRequestHandler<GetTrailBySlugQuery, TrailDetailDto?>
{
    private readonly ITrailRepository _trailRepository;
    private readonly IMapper _mapper;
    private readonly GeometryFactory _geometryFactory;

    public GetTrailBySlugQueryHandler(
        ITrailRepository trailRepository,
        IMapper mapper)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
        _geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    }


    public async Task<TrailDetailDto?> Handle(
        GetTrailBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var trail = await _trailRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (trail == null)
        {
            throw new TrailNotFoundException(request.Slug);
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
        trailDetailDto.DistanceToUserMeters = trail.RouteGeom?.Distance(userLocationPoint);
        trailDetailDto.DistanceToUserKm = trailDetailDto.DistanceToUserMeters / 1000;

        return trailDetailDto;
    }
}
