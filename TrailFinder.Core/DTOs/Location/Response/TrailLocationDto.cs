using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Location.Response;

public class TrailLocationDto
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
        LocationDto locationDto
    )
    {
        Id = id;
        TrailId = trailId;
        LocationId = locationId;
        LocationType = locationType;
        Comment = comment;
        DisplayOrder = displayOrder;
        Location = locationDto;
    }

    public Guid Id { get; private set; }
    public Guid TrailId { get; private set; }
    public Guid LocationId { get; private set; }

    //public TrailDto Trail { get; private set; }


    public LocationType LocationType { get; private set; }
    public string Comment { get; private set; } = null!;
    public int DisplayOrder { get; set; }
    public LocationDto Location { get; set; }
}