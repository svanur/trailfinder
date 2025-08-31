// TrailFinder.Application/Features/GpxFiles/Queries/GetGpxFileMetadata/GpxFileMetadataDto.cs
namespace TrailFinder.Core.DTOs.GpxFile.Responses;

public record GpxFileMetadataDto(
    Guid Id,
    Guid TrailId,
    string StoragePath,
    string OriginalFileName,
    string FileName,
    long FileSize,
    string ContentType,
    bool isActive,
    Guid createdBy,
    DateTime createdAt
);
