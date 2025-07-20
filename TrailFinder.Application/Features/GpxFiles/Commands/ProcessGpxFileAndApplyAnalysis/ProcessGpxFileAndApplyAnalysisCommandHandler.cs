// TrailFinder.Application/Features/GpxFiles/Commands/ProcessGpxFileAndApplyAnalysis/ProcessGpxFileAndApplyAnalysisCommandHandler.cs
using MediatR;
using Microsoft.Extensions.Logging;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;
using AutoMapper;

namespace TrailFinder.Application.Features.GpxFiles.Commands.ProcessGpxFileAndApplyAnalysis;

public class ProcessGpxFileAndApplyAnalysisCommandHandler(
    IGpxFileRepository gpxFileRepository,
    ITrailRepository trailRepository,
    ISupabaseStorageService storageService,
    ILogger<ProcessGpxFileAndApplyAnalysisCommandHandler> logger,
    IMapper mapper)
    : IRequestHandler<ProcessGpxFileAndApplyAnalysisCommand, Guid>
{
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(
        ProcessGpxFileAndApplyAnalysisCommand request, 
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Processing GPX file analysis for Trail ID: {TrailId}", request.TrailId);

        // --- Step 1: Update/Create GpxFile Metadata ---
        Guid gpxFileMetadataId;
        var existingGpxFile = await gpxFileRepository.GetByTrailIdAsync(request.TrailId, cancellationToken);

        if (existingGpxFile != null)
        {
            logger.LogInformation("Existing GPX file metadata found for Trail ID: {TrailId}. Updating...", request.TrailId);

            // Optional: Delete old blob from storage if the storage path is changing or it's a new file entirely
            // If the storagePath might remain the same but content changes, Supabase will handle overwrite.
            // If you want to ensure old file is gone:
            if (existingGpxFile.StoragePath != request.StoragePath)
            {
                 logger.LogInformation("Deleting old GPX file from storage: {StoragePath}", existingGpxFile.StoragePath);
                 await storageService.DeleteGpxFileAsync(existingGpxFile.StoragePath);
            }

            // Update existing metadata
            existingGpxFile.StoragePath = request.StoragePath;
            existingGpxFile.OriginalFileName = request.OriginalFileName;
            existingGpxFile.FileName = request.FileName;
            existingGpxFile.FileSize = request.FileSize;
            existingGpxFile.ContentType = request.ContentType;
            existingGpxFile.IsActive = true; // Ensure it's active after an upload
            existingGpxFile.UpdatedBy = request.CreatedBy; // Assuming updater is the creator for first upload/replacement
            existingGpxFile.UpdatedAt = DateTime.UtcNow; // Trigger will also set this

            await gpxFileRepository.UpdateAsync(existingGpxFile, cancellationToken);
            gpxFileMetadataId = existingGpxFile.Id;
            logger.LogInformation("GPX file metadata updated successfully for Trail ID: {TrailId}, GpxFile ID: {GpxFileId}", request.TrailId, gpxFileMetadataId);
        }
        else
        {
            logger.LogInformation("No existing GPX file metadata found for Trail ID: {TrailId}. Creating new record...", request.TrailId);
            var newGpxFile = new GpxFile(
                trailId: request.TrailId,
                storagePath: request.StoragePath,
                originalFileName: request.OriginalFileName,
                fileName: request.FileName,
                fileSize: request.FileSize,
                contentType: request.ContentType,
                isActive: true,
                createdBy: request.CreatedBy,
                createdAt: DateTime.UtcNow
            );
            var createdGpxFile = await gpxFileRepository.CreateAsync(newGpxFile, cancellationToken);
            gpxFileMetadataId = createdGpxFile.Id;
            logger.LogInformation("New GPX file metadata created successfully for Trail ID: {TrailId}, GpxFile ID: {GpxFileId}", request.TrailId, gpxFileMetadataId);
        }

        // --- Step 2: Update Trail Entity with Analysis Results ---
        var trailToUpdate = await trailRepository.GetByIdAsync(request.TrailId, cancellationToken);
        if (trailToUpdate == null)
        {
            // This should ideally not happen if GetTrailQuery succeeded in the controller
            logger.LogError("Trail with ID {TrailId} not found during GPX analysis update. This indicates a data inconsistency.", request.TrailId);
            throw new TrailNotFoundException(request.TrailId);
        }

        logger.LogInformation("Updating Trail ID: {TrailId} with analysis results...", request.TrailId);
        trailToUpdate.DistanceMeters = request.AnalyzedDistance;
        trailToUpdate.ElevationGainMeters = request.AnalyzedElevationGain;
        trailToUpdate.DifficultyLevel = request.AnalyzedDifficultyLevel;
        trailToUpdate.RouteType = request.AnalyzedRouteType;
        trailToUpdate.TerrainType = request.AnalyzedTerrainType;
        trailToUpdate.RouteGeom = request.AnalyzedRouteGeom;
        
        // Set updated_by and updated_at on the Trail entity itself
        trailToUpdate.UpdatedBy = request.CreatedBy; // Assuming the uploader is the updater
        trailToUpdate.UpdatedAt = DateTime.UtcNow; // Trigger will also set this

        await trailRepository.UpdateAsync(trailToUpdate, cancellationToken);
        logger.LogInformation("Trail ID: {TrailId} updated successfully with analysis results.", request.TrailId);

        return gpxFileMetadataId;
    }
}