// TrailFinder.Application/Features/GpxFiles/Queries/GetGpxFileMetadata/GetGpxFileMetadataQuery.cs

using AutoMapper;
using MediatR;
using TrailFinder.Core.Interfaces.Repositories;

public record GetGpxFileMetadataQuery(Guid TrailId) : IRequest<GpxFileMetadataDto?>; // Returns a DTO or null

// TrailFinder.Application/Features/GpxFiles/Queries/GetGpxFileMetadata/GpxFileMetadataDto.cs
public record GpxFileMetadataDto(
    Guid Id,
    Guid TrailId,
    string StoragePath,
    string OriginalFileName,
    string FileName,
    long FileSize,
    string ContentType
    // Add other relevant metadata
);

// TrailFinder.Application/Features/GpxFiles/Queries/GetGpxFileMetadata/GetGpxFileMetadataQueryHandler.cs
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
