using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

// Matches: CREATE TYPE location_type AS ENUM ('unknown', 'start', 'aid_station', 'checkpoint', 'end');

public enum LocationType
{
    [PgName("unknown")] // This maps the PostgreSQL 'unknown' string to this C# enum member
    Unknown = 0, // It's good practice for 'unknown' or default to be 0
    
    [PgName("start")]
    Start = 1,
    
    [PgName("aid_station")]
    AidStation = 2,
    
    [PgName("checkpoint")]
    Checkpoint = 3,
    
    [PgName("end")]
    End = 4
}