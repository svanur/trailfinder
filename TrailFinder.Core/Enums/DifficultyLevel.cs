using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum DifficultyLevel
{
    [PgName("unknown")]
    Unknown,
    
    [PgName("easy")]
    Easy,
    
    [PgName("moderate")]
    Moderate,
    
    [PgName("hard")]
    Hard,
    
    [PgName("extreme")]
    Extreme
}
