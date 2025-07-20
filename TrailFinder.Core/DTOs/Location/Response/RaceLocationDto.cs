using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Location.Response;

public class RaceLocationDto : BaseDto
{
    private RaceLocationDto()
    {
    }

    public RaceLocationDto(
        Guid id,
        Guid raceId,
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
        RaceId = raceId;
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
    public Guid RaceId { get; private set; }
    public Guid LocationId { get; private set; }
    public LocationType LocationType { get; private set; }
    public string Comment { get; private set; } = null!;
    public LocationLiteDto? Location { get; set; }
}