using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandler : IRequestHandler<GetTrailQuery, TrailDto>
{
    private readonly ILogger<GetTrailQueryHandler> _logger;
    private readonly IMapper _mapper;
    
    private readonly ILocationRepository _locationRepository;
    private readonly ITrailRepository _trailRepository;

    public GetTrailQueryHandler(
        ILogger<GetTrailQueryHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository,
        ITrailLocationRepository trailLocationRepository,
        ILocationRepository locationRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
        _locationRepository = locationRepository;
    }

    public async Task<TrailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        // Use the new method that includes related data
        var trail = await _trailRepository.GetByIdWithLocationsAsync(request.Id, cancellationToken);
        if (trail == null)
        {
            throw new TrailNotFoundException(request.Id);
        }

        // AutoMapper should be configured to map the nested collections
        var trailDto = _mapper.Map<TrailDto>(trail);
        
        return trailDto;
    }
}