// TrailFinder.Application/Features/GpxFiles/Commands/ProcessGpxFile/ProcessGpxFileCommandHandler.cs

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Application.Features.GpxFiles.Commands.ProcessGpxFile;

public class ProcessGpxFileCommandHandler(
    IGpxFileRepository gpxFileRepository,
    ITrailRepository trailRepository,
    ISupabaseStorageService storageService,
    ILogger<ProcessGpxFileCommandHandler> logger,
    IMapper mapper)
    : IRequestHandler<ProcessGpxFileCommand, Guid>
{
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(
        ProcessGpxFileCommand processGpxFileCommandRequest, 
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Processing GPX file for Trail ID: {TrailId}", processGpxFileCommandRequest.TrailId);

        // --- Step 1: Update/Create GpxFile Metadata ---
        Guid gpxFileMetadataId;
        var existingGpxFile = await gpxFileRepository.GetByTrailIdAsync(processGpxFileCommandRequest.TrailId, cancellationToken);

        if (existingGpxFile != null)
        {
            logger.LogInformation("Existing GPX file metadata found for Trail ID: {TrailId}. Updating...", processGpxFileCommandRequest.TrailId);

            // Optional: Delete old blob from storage if the storage path is changing or it's a new file entirely
            // If the storagePath might remain the same but content changes, Supabase will handle overwrite.
            // If you want to ensure old file is gone:
            if (existingGpxFile.StoragePath != processGpxFileCommandRequest.StoragePath)
            {
                 logger.LogInformation("Deleting old GPX file from storage: {StoragePath}", existingGpxFile.StoragePath);
                 await storageService.DeleteGpxFileAsync(existingGpxFile.StoragePath);
            }

            // Update existing metadata
            existingGpxFile.StoragePath = processGpxFileCommandRequest.StoragePath;
            existingGpxFile.OriginalFileName = processGpxFileCommandRequest.OriginalFileName;
            existingGpxFile.FileName = processGpxFileCommandRequest.FileName;
            existingGpxFile.FileSize = processGpxFileCommandRequest.FileSize;
            existingGpxFile.ContentType = processGpxFileCommandRequest.ContentType;
            existingGpxFile.IsActive = true; // Ensure it's active after an upload
            existingGpxFile.UpdatedBy = processGpxFileCommandRequest.CreatedBy; // Assuming updater is the creator for first upload/replacement
            existingGpxFile.UpdatedAt = DateTime.UtcNow; // Trigger will also set this

            await gpxFileRepository.UpdateAsync(existingGpxFile, cancellationToken);
            gpxFileMetadataId = existingGpxFile.Id;
            logger.LogInformation("GPX file metadata updated successfully for Trail ID: {TrailId}, GpxFile ID: {GpxFileId}", processGpxFileCommandRequest.TrailId, gpxFileMetadataId);
        }
        else
        {
            logger.LogInformation("No existing GPX file metadata found for Trail ID: {TrailId}. Creating new record...", processGpxFileCommandRequest.TrailId);
            var newGpxFile = new GpxFile(
                trailId: processGpxFileCommandRequest.TrailId,
                storagePath: processGpxFileCommandRequest.StoragePath,
                originalFileName: processGpxFileCommandRequest.OriginalFileName,
                fileName: processGpxFileCommandRequest.FileName,
                fileSize: processGpxFileCommandRequest.FileSize,
                contentType: processGpxFileCommandRequest.ContentType,
                isActive: true,
                createdBy: processGpxFileCommandRequest.CreatedBy,
                createdAt: DateTime.UtcNow
            );
            var createdGpxFile = await gpxFileRepository.CreateAsync(newGpxFile, cancellationToken);
            gpxFileMetadataId = createdGpxFile.Id;
            logger.LogInformation("New GPX file metadata created successfully for Trail ID: {TrailId}, GpxFile ID: {GpxFileId}", processGpxFileCommandRequest.TrailId, gpxFileMetadataId);
        }

        
        // --- Step 2: Update Trail Entity with RouteGeom // not Analysis Results ---
        var trailToUpdate = await trailRepository.GetByIdAsync(processGpxFileCommandRequest.TrailId, cancellationToken);
        if (trailToUpdate == null)
        {
            // This should ideally not happen if GetTrailQuery succeeded in the controller
            logger.LogError("Trail with ID {TrailId} not found during GPX analysis update. This indicates a data inconsistency.", processGpxFileCommandRequest.TrailId);
            throw new TrailNotFoundException(processGpxFileCommandRequest.TrailId);
        }

        logger.LogInformation("Updating Trail ID: {TrailId} with analysis results...", processGpxFileCommandRequest.TrailId);
        //trailToUpdate.DistanceMeters = processGpxFileCommandRequest.AnalyzedDistance;
        //trailToUpdate.ElevationGainMeters = processGpxFileCommandRequest.AnalyzedElevationGain;
        
        trailToUpdate.DifficultyLevel = processGpxFileCommandRequest.AnalyzedDifficultyLevel;
        trailToUpdate.RouteType = processGpxFileCommandRequest.AnalyzedRouteType;
        trailToUpdate.TerrainType = processGpxFileCommandRequest.AnalyzedTerrainType;
        trailToUpdate.RouteGeom = processGpxFileCommandRequest.AnalyzedRouteGeom;
        
        // Set updated_by and updated_at on the Trail entity itself
        trailToUpdate.UpdatedBy = processGpxFileCommandRequest.CreatedBy; // Assuming the uploader is the updater
        trailToUpdate.UpdatedAt = DateTime.UtcNow; // Trigger will also set this

        await trailRepository.UpdateAsync(trailToUpdate, cancellationToken);
        logger.LogInformation("Trail ID: {TrailId} updated successfully with analysis results.", processGpxFileCommandRequest.TrailId);
        
        return gpxFileMetadataId;
    }
}