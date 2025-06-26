using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum DifficultyLevel
{
    [PgName("easy")]
    Easy = 0,
    
    [PgName("moderate")]
    Moderate = 1,
    
    [PgName("hard")]
    Hard = 2,
    
    [PgName("extreme")]
    Extreme = 3,
    
    [PgName("unknown")]
    Unknown = 4
}
