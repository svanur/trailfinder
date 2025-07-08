using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Location.Response;

public class RaceLocationDto
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
        LocationLiteDto locationDto
    )
    {
        Id = id;
        RaceId = raceId;
        LocationId = locationId;
        LocationType = locationType;
        Comment = comment;
        DisplayOrder = displayOrder;
        Location = locationDto;
    }

    public Guid Id { get; private set; }
    public Guid RaceId { get; private set; }
    public Guid LocationId { get; private set; }

    //public TrailDto Trail { get; private set; }


    public LocationType LocationType { get; private set; }
    public string Comment { get; private set; } = null!;
    public int DisplayOrder { get; set; }
    public LocationLiteDto Location { get; set; }
}