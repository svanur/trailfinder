using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum DifficultyLevel
{
    [PgName("easy")]
    Easy,
    
    [PgName("moderate")]
    Moderate,
    
    [PgName("hard")]
    Hard,
    
    [PgName("extreme")]
    Extreme,
    
    [PgName("unknown")]
    Unknown
}

/*
 *
 public enum DifficultyLevel
{
    Easy,
    Moderate,
    Challenging,
    Difficult,
    VeryDifficult
}

 */