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
    private readonly ITrailLocationRepository _trailLocationRepository;
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
        _trailLocationRepository = trailLocationRepository;
        _locationRepository = locationRepository;
    }

    public async Task<TrailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        //var trail = await _context.Set<Core.Entities.Trail>()
        //  .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        var trail = await _trailRepository.GetByIdAsync(request.Id, cancellationToken);
        if (trail == null) throw new TrailNotFoundException(request.Id);

        var trailDto = _mapper.Map<TrailDto>(trail);

        var trailLocations = await _trailLocationRepository.GetByTrailIdAsync(trailDto.Id, cancellationToken);
        
        foreach (var trailLocation in trailLocations)
        {
            //trailLocation.Location = await _locationRepository.GetByIdAsync(trailLocation.LocationId, cancellationToken);
        }
        
        trailDto.locations = _mapper.Map<IEnumerable<TrailLocationDto>>(trailLocations);

        return trailDto;
    }
}