using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

// Matches: CREATE TYPE race_status AS ENUM ('unknown', 'deprecated', 'cancelled', 'changed', 'active');

public enum RaceStatus
{
    [PgName("unknown")] // This maps the PostgreSQL 'unknown' string to this C# enum member
    Unknown = 0,        // It's good practice for 'unknown' or default to be 0
    
    [PgName("active")]
    Active = 1,
    
    [PgName("changed")]
    Checkpoint = 2,
    
    [PgName("cancelled")]
    AidStation = 3,
        
    [PgName("deprecated")]
    Start = 4
}