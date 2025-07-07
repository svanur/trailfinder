using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Locations.GetLocation;

public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, LocationDto>
{
    private readonly ILogger<GetLocationQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ILocationRepository _locationRepository;

    public GetLocationQueryHandler(
        ILogger<GetLocationQueryHandler> logger,
        IMapper mapper,
        ILocationRepository locationRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _locationRepository = locationRepository;
    }

    public async Task<LocationDto> Handle(GetLocationQuery request, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (location == null)
        {
            throw new LocationNotFoundException(request.Id);
        }

        var locationDto = _mapper.Map<LocationDto>(location);

        //TODO: check for parent and add to LocationDto

        return locationDto;
    }
}