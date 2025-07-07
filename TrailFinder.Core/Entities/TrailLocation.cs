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
        TrailId = trailId; // Correct assignment
        LocationId = locationId; // Correct assignment
        LocationType = locationType; // Correct assignment
        Comment = comment; // Correct assignment
        DisplayOrder = displayOrder; // Correct assignment

        CreatedAt = DateTime.UtcNow; // Or set to default if handled by DB
        UpdatedAt = DateTime.UtcNow; // Or set to default if handled by DB
    }

    public Guid Id { get; private set; } // Assuming Id is the PK, based on your config
    public Guid TrailId { get; private set; }
    public Guid LocationId { get; private set; }

    public LocationType LocationType { get; private set; }
    public string? Comment { get; private set; } // Make nullable to match DB schema
    public int? DisplayOrder { get; set; } // Make nullable to match DB schema (numeric null)

    // Navigation properties for relationships (if you have them configured in DbContext)
    public Trail Trail { get; private set; } = null!;
    public Location? Location { get; set; } = null!;

    // You might want methods to update these values if they are mutable
    public void UpdateDetails(LocationType? locationType = null, string? comment = null, int? displayOrder = null)
    {
        if (locationType.HasValue) LocationType = locationType.Value;
        if (comment != null) Comment = comment; // Allow setting to null explicitly
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;
        // Or if you pass null, you might mean "don't change"
        // if (comment is not null) Comment = comment; // Use this if null means "don't change"
        // else if (comment is string s) Comment = s; // Use this if null means "set to null"
        UpdatedAt = DateTime.UtcNow;
    }
}