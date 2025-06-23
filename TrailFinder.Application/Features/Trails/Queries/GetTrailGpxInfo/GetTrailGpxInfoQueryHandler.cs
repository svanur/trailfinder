using MediatR;
using TrailFinder.Application.Services;
using TrailFinder.Core.DTOs.Gpx.Responses;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;

public class GetTrailGpxInfoQueryHandler : IRequestHandler<GetTrailGpxInfoQuery, GpxInfoDto>
{
    private readonly ITrailRepository _trailRepository;
    private readonly IGpxService _gpxService;
    private readonly ISupabaseStorageService _storageService;

    public GetTrailGpxInfoQueryHandler(
        ITrailRepository trailRepository,
        IGpxService gpxService,
        ISupabaseStorageService storageService
    )
    {
        _trailRepository = trailRepository;
        _gpxService = gpxService;
        _storageService = storageService;
    }

    public async Task<GpxInfoDto> Handle(GetTrailGpxInfoQuery request, CancellationToken cancellationToken)
    {
        var trail = await _trailRepository.GetByIdAsync(request.TrailId, cancellationToken);
        if (trail == null)
        {
            throw new TrailNotFoundException(request.TrailId);
        }

        await using var gpxStream = await _storageService.GetGpxFileFromStorage(trail.Id, trail.Slug);
        return await _gpxService.ExtractGpxInfo(gpxStream);
    }
}
