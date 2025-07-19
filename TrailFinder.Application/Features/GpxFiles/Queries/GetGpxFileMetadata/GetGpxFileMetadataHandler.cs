using AutoMapper;
using MediatR;
using TrailFinder.Core.DTOs.GpxFile.Responses;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Application.Features.GpxFiles.Queries.GetGpxFileMetadata;

public class GetGpxFileMetadataQueryHandler : IRequestHandler<GetGpxFileMetadataQuery, GpxFileMetadataDto?>
{
    private readonly IGpxFileRepository _gpxFileRepository; // Assuming this repository also queries metadata
    private readonly IMapper _mapper;

    public GetGpxFileMetadataQueryHandler(IGpxFileRepository gpxFileRepository, IMapper mapper)
    {
        _gpxFileRepository = gpxFileRepository;
        _mapper = mapper;
    }

    public async Task<GpxFileMetadataDto?> Handle(GetGpxFileMetadataQuery request, CancellationToken cancellationToken)
    {
        var gpxFileEntity = await _gpxFileRepository.GetByTrailIdAsync(request.TrailId, cancellationToken);
        return _mapper.Map<GpxFileMetadataDto>(gpxFileEntity);
    }
}