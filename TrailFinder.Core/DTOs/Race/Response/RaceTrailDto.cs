using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Race.Response;

public class RaceTrailDto
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
        DateTime createdAt,
        DateTime updatedAt,
        Guid userId
    )
    {
        Id = id;
        RaceId = raceId;
        TrailId = trailId;
        RaceStatus = raceStatus;
        Comment = comment;
        DisplayOrder = displayOrder;

        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }

    public Guid Id { get; set; }

    public Guid RaceId { get; set; }
    public Guid TrailId { get; set; }
    public RaceStatus RaceStatus { get; set; }
    public string Comment { get; set; }
    public int DisplayOrder { get; set; }
    public string WebUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
}