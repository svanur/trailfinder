using MediatR;
using TrailFinder.Application.Services;
using TrailFinder.Core.DTOs.Gpx;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;

public class GetTrailGpxInfoQueryHandler : IRequestHandler<GetTrailGpxInfoQuery, GpxInfoDto>
{
    private readonly IGpxService _gpxService;

    public GetTrailGpxInfoQueryHandler(IGpxService gpxService)
    {
        _gpxService = gpxService;
    }

    public async Task<GpxInfoDto> Handle(GetTrailGpxInfoQuery request, CancellationToken cancellationToken)
    {
        await using var gpxStream = await _gpxService.GetGpxFileFromStorage(request.TrailId);
        return await _gpxService.ExtractGpxInfo(gpxStream);
    }
}
