using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class TrailLocation : BaseEntity
{
    private TrailLocation()
    {
    } // For EF Core

    public TrailLocation(
        Guid id,
        Guid trailId,
        Guid locationId,
        LocationType locationType,
        string? comment, // Make parameter nullable
        int? displayOrder // Make parameter nullable
    )
    {
        Id = id;
        TrailId = trailId;
        LocationId = locationId;
        LocationType = locationType;
        Comment = comment;
        DisplayOrder = displayOrder;

        // Or set to default if handled by DB
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid TrailId { get; private set; }
    
    public Guid LocationId { get; private set; } // Foreign Key Property
    public Location Location { get; set; } = null!; // Navigation Property
    
    public LocationType LocationType { get; private set; }
    public string? Comment { get; private set; }
    public int? DisplayOrder { get; set; }

    // Navigation properties for relationships (if you have them configured in DbContext)
    public Trail Trail { get; private set; } = null!;

    // You might want methods to update these values if they are mutable
    public void UpdateDetails(LocationType? locationType = null, string? comment = null, int? displayOrder = null)
    {
        if (locationType.HasValue) LocationType = locationType.Value;
        if (comment != null) Comment = comment; // Allow setting to null explicitly
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        
        UpdatedAt = DateTime.UtcNow;
    }
}