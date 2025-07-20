// TrailFinder.Application/Features/GpxFiles/Commands/CreateGpxFileMetadata/CreateGpxFileMetadataCommandHandler.cs
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.Interfaces; // Assuming IGpxFileRepository
using TrailFinder.Core.Entities; // Assuming GpxFile entity
using AutoMapper;
using TrailFinder.Core.Interfaces.Repositories; // If you use AutoMapper for command to entity mapping

namespace TrailFinder.Application.Features.GpxFiles.Commands.CreateGpxFileMetadata;

public class CreateGpxFileMetadataCommandHandler : IRequestHandler<CreateGpxFileMetadataCommand, Guid>
{
    private readonly IGpxFileRepository _gpxFileRepository;
    private readonly ILogger<CreateGpxFileMetadataCommandHandler> _logger;
    private readonly IMapper _mapper; // Optional, if you map command to entity

    public CreateGpxFileMetadataCommandHandler(
        IGpxFileRepository gpxFileRepository,
        ILogger<CreateGpxFileMetadataCommandHandler> logger,
        IMapper mapper) // Inject IMapper
    {
        _gpxFileRepository = gpxFileRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateGpxFileMetadataCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating GPX file metadata for Trail ID: {request.TrailId}");

        // Map the command to your GpxFile entity
        var gpxFileEntity = _mapper.Map<GpxFile>(request);

        // Set server-controlled audit fields
        gpxFileEntity.CreatedAt = DateTime.UtcNow;
        gpxFileEntity.IsActive = true; // Ensure it's active on creation

        Guid newGpxFileId;
        try
        {
            var createdGpxFile = await _gpxFileRepository.CreateAsync(gpxFileEntity, cancellationToken);
            newGpxFileId = createdGpxFile.Id;
            _logger.LogInformation($"GPX file metadata created successfully with ID: {newGpxFileId} for Trail ID: {request.TrailId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create GPX file metadata for Trail ID: {request.TrailId}.");
            throw new InvalidOperationException($"Failed to create GPX file metadata.", ex);
        }

        return newGpxFileId;
    }
}
