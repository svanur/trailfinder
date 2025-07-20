// TrailFinder.Core/Entities/GpxFile.cs

using TrailFinder.Core.Entities.Common;

namespace TrailFinder.Core.Entities;

public class GpxFile : BaseEntity
{
    private GpxFile()
    {
    } // For EF Core

    // Updated Constructor
    public GpxFile(
        Guid trailId,
        string storagePath,
        string originalFileName,
        string fileName,
        long fileSize, // Added fileSize as per schema
        string contentType,
        bool isActive,
        Guid createdBy, // Added createdBy
        DateTime createdAt // Added createdAt
    )
    {
        TrailId = trailId;
        StoragePath = storagePath;
        OriginalFileName = originalFileName;
        FileName = fileName;
        FileSize = fileSize; // Set fileSize
        ContentType = contentType;
        IsActive = isActive;

        // Set BaseEntity properties
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        // Id will be set by the database or repository's AddAsync
        // UpdatedAt and UpdatedBy are nullable and typically set on subsequent updates or by triggers
    }

    public Guid TrailId { get; set; }
    public string StoragePath { get; set; }
    public string OriginalFileName { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; } // Added this property as per schema
    public string ContentType { get; set; }
    public bool IsActive { get; set; }
}