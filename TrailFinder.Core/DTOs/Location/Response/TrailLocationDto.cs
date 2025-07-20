using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Location.Response;

public class TrailLocationDto : BaseDto
{
    private TrailLocationDto()
    {
    }

    public TrailLocationDto(
        Guid id,
        Guid trailId,
        Guid locationId,
        LocationType locationType,
        string comment,
        int displayOrder,
        LocationLiteDto locationDto,
        Guid createdBy,
        DateTime createdAt,
        Guid? updatedBy,
        DateTime? updatedAt
    )
    {
        Id = id;
        TrailId = trailId;
        LocationId = locationId;
        LocationType = locationType;
        Comment = comment;
        DisplayOrder = displayOrder;
        Location = locationDto;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }

    
    public int DisplayOrder { get; set; }
    public Guid TrailId { get; private set; }
    public Guid LocationId { get; private set; }
    public LocationType LocationType { get; private set; }
    public string Comment { get; private set; } = null!;
    public LocationLiteDto Location { get; set; }
}
