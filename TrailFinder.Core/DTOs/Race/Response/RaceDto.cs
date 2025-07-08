using TrailFinder.Core.DTOs.Location.Response;

namespace TrailFinder.Core.DTOs.Race.Response;

public class RaceDto
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
        bool isActive,
        int recurringMonth,
        int recurringWeekOrdinal,
        int recurringWeekday,
        DateTime createdAt,
        DateTime updatedAt,
        Guid userId
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        WebUrl = webUrl;
        IsActive = isActive;

        RecurringMonth = recurringMonth;
        RecurringWeekOrdinal = recurringWeekOrdinal;
        RecurringWeekday = recurringWeekday;

        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? WebUrl { get; set; }
    public bool IsActive { get; set; }

    private int RecurringMonth { get; set; }
    private int RecurringWeekOrdinal { get; set; }
    private int RecurringWeekday { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }

    public IEnumerable<RaceLocationDto> RaceLocations { get; set; }
    
}