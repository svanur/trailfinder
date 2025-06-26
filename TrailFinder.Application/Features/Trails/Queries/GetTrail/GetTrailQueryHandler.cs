using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Exceptions;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public class GetTrailQueryHandler : IRequestHandler<GetTrailQuery, TrailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TrailDto> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        var trail = await _context.Set<Core.Entities.Trail>()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trail == null)
        {
            throw new TrailNotFoundException(request.Id);
        }

        return _mapper.Map<TrailDto>(trail);
    }
}