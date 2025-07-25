using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Race.Response;

public class RaceDto : BaseDto
{
    public RaceDto()
    {
    }

    public RaceDto(
        Guid id,
        string name,
        string slug,
        string description,
        string webUrl,
        RaceStatus raceStatus,
        int recurringMonth,
        int recurringWeek,
        int recurringWeekday,
        Guid createdBy,
        DateTime createdAt,
        Guid? updatedBy,
        DateTime? updatedAt
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        WebUrl = webUrl;
        RaceStatus = raceStatus;

        RecurringMonth = recurringMonth;
        RecurringWeek = recurringWeek;
        RecurringWeekday = recurringWeekday;

        CreatedBy = createdBy;
        CreatedAt = createdAt;
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }

    public Guid LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? WebUrl { get; set; }
    public RaceStatus RaceStatus { get; set; }

    private int RecurringMonth { get; set; }
    private int RecurringWeek { get; set; }
    private int RecurringWeekday { get; set; }

    public IEnumerable<RaceLocationDto>? RaceLocations { get; set; }
}