using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailBySlug;

public class GetTrailBySlugQueryHandler : IRequestHandler<GetTrailBySlugQuery, TrailDetailDto?>
{
    private readonly ITrailRepository _trailRepository;
    private readonly IMapper _mapper;

    public GetTrailBySlugQueryHandler(
        ITrailRepository trailRepository,
        IMapper mapper)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
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
        
        return _mapper.Map<TrailDetailDto>(trail);
    }
}
