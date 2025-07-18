using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Race.Response;

public class RaceTrailDto : BaseDto
{
    public RaceTrailDto()
    {
    }

    public RaceTrailDto(
        Guid id,
        Guid raceId,
        Guid trailId,
        RaceStatus raceStatus,
        string comment,
        int displayOrder,
        
        Guid createdBy,
        DateTime createdAt,
        Guid updatedBy,
        DateTime updatedAt
    )
    {
        Id = id;
        RaceId = raceId;
        TrailId = trailId;
        RaceStatus = raceStatus;
        Comment = comment;
        DisplayOrder = displayOrder;

        CreatedBy = createdBy;
        CreatedAt = createdAt;
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }
    public Guid RaceId { get; set; }
    public Guid TrailId { get; set; }
    public RaceStatus RaceStatus { get; set; }
    public string Comment { get; set; }
    public int DisplayOrder { get; set; }
}