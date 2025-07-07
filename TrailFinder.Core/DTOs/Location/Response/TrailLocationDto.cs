using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Location.Response;

public class TrailLocationDto
{
    private TrailLocationDto()
    {
    }

    public TrailLocationDto(
        //TrailDto trailDto,
        LocationType locationType,
        string comment,
        int displayOrder,
        LocationLiteDto locationDto
    )
    {
        //Trail = trailDto;
        LocationType = locationType;
        Comment = comment;
        DisplayOrder = displayOrder;
        Location = locationDto;
    }

    public Guid Id { get; private set; }

    //public TrailDto Trail { get; private set; }


    public LocationType LocationType { get; private set; }
    public string Comment { get; private set; } = null!;
    public int DisplayOrder { get; set; }
    public LocationLiteDto Location { get; private set; }
}