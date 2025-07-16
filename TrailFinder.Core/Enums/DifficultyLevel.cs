using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum DifficultyLevel
{
    [PgName("unknown")]
    Unknown = 0,
        
    [PgName("easy")]
    Easy = 1,
    
    [PgName("moderate")]
    Moderate = 2,
    
    [PgName("hard")]
    Hard = 3,
    
    [PgName("extreme")]
    Extreme = 4
    
    
}
