using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Locations.GetLocations;

public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, PaginatedResult<LocationDto>>
{
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<GetLocationsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetLocationsQueryHandler(
        ILogger<GetLocationsQueryHandler> logger,
        IMapper mapper,
        ILocationRepository locationRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _locationRepository = locationRepository;
    }

    public async Task<PaginatedResult<LocationDto>> Handle(
        GetLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var locations = await _locationRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<PaginatedResult<LocationDto>>(locations);
    }
}