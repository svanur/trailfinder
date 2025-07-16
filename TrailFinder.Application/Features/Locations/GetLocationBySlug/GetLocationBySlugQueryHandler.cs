using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Locations.GetLocationBySlug;

public class GetLocationBySlugQueryHandler : IRequestHandler<GetLocationBySlugQuery, LocationDto?>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public GetLocationBySlugQueryHandler(
        ILocationRepository locationRepository,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }


    public async Task<LocationDto?> Handle(
        GetLocationBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (location == null) throw new LocationNotFoundException(request.Slug);

        return _mapper.Map<LocationDto>(location);
    }
}