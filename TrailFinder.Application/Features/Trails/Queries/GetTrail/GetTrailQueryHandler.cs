using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandler : IRequestHandler<GetTrailQuery, TrailDetailDto>
{
    private readonly ILogger<GetTrailQueryHandler> _logger;
    private readonly IMapper _mapper;

    private readonly ITrailRepository _trailRepository;

    public GetTrailQueryHandler(
        ILogger<GetTrailQueryHandler> logger,
        IMapper mapper,
        ITrailRepository trailRepository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _trailRepository = trailRepository;
    }

    public async Task<TrailDetailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        var trail = await _trailRepository.GetByIdWithLocationsAsync(request.Id, cancellationToken);
        if (trail == null)
        {
            throw new TrailNotFoundException(request.Id);
        }

        return _mapper.Map<TrailDetailDto>(trail);
    }
}
