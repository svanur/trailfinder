using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;

public class GetTrailsByParentIdQueryHandler  : IRequestHandler<GetTrailsByParentIdQuery, PaginatedResult<TrailDto>>
{
    private readonly ITrailRepository _trailRepository;
    private readonly IMapper _mapper;

    public GetTrailsByParentIdQueryHandler(
        ITrailRepository trailRepository,
        IMapper mapper)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<TrailDto>> Handle(
        GetTrailsByParentIdQuery request,
        CancellationToken cancellationToken)
    {
        
        var trails = await _trailRepository.GetFilteredAsync(
            new TrailFilterDto { ParentId = request.ParentId }, cancellationToken
        );
        
        return _mapper.Map<PaginatedResult<TrailDto>>(trails);
    }
}
