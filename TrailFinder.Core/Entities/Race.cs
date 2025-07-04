using System;

namespace TrailFinder.Core.Entities
{
    public class Race
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; } // For permanently discontinued races

        // Recurring Date Information
        public int RecurringMonth { get; set; } // 1-12
        public int RecurringWeekOrdinal { get; set; } // 1-5 (or higher, depending on how you define 'last')
        public int RecurringWeekday { get; set; } // 1-7 (1=Monday, 7=Sunday)

        // Navigation property for race_trails (optional, but good for ORM)
        // public ICollection<RaceTrail> RaceTrails { get; set; }
    }
}