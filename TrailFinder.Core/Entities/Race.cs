using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class Race : BaseEntity
{
    private Race()
    {
    } // For EF Core

    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string WebUrl { get; set; }
    public RaceStatus RaceStatus { get; set; }

    // Recurring Date Information
    public int RecurringMonth { get; set; } // 1-12
    public int RecurringWeek { get; set; } // 1-5 (or higher, depending on how you define 'last')
    public int RecurringWeekday { get; set; } // 1-7 (1=Monday, 7=Sunday)

    // Navigation property for race_trails (optional, but good for ORM)
    // public ICollection<RaceTrail> RaceTrails { get; set; }


    // For EF Core, to understand the relationships and use .Include(),
    // navigation properties are needed in the entity classes
    public ICollection<RaceLocation> RaceLocations { get; private set; } = new List<RaceLocation>();
    public ICollection<RaceTrail> RaceTrails { get; private set; } = new List<RaceTrail>();
}