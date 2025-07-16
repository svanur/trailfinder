using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

// Matches: CREATE TYPE race_status AS ENUM ('unknown', 'deprecated', 'cancelled', 'changed', 'active');

public enum RaceStatus
{
    [PgName("unknown")] // This maps the PostgreSQL 'unknown' string to this C# enum member
    Unknown = 0, // It's good practice for 'unknown' or default to be 0
    
    [PgName("deprecated")]
    Start = 1,
    
    [PgName("cancelled")]
    AidStation = 2,
    
    [PgName("changed")]
    Checkpoint = 3,
    
    [PgName("active")]
    Active = 5
}