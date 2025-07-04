using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandler : IRequestHandler<GetTrailQuery, TrailDto>
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

    public async Task<TrailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        //var trail = await _context.Set<Core.Entities.Trail>()
          //  .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        var trail = await _trailRepository.GetByIdAsync(request.Id, cancellationToken);
        if (trail == null)
        {
            throw new TrailNotFoundException(request.Id);
        }

        var trailDto = _mapper.Map<TrailDto>(trail);
        //TODO: add values to trailDto
        return trailDto;
    }
}