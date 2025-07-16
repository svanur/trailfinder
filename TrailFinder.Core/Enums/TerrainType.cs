using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum TerrainType
{
    [PgName("unknown")]
    Unknown = 0,
    
    [PgName("flat")]
    Flat = 1,           // Minimal elevation changes
    
    [PgName("rolling")]
    Rolling = 2,        // Moderate ups and downs
    
    [PgName("hilly")]
    Hilly = 3,          // Significant elevation changes
    
    [PgName("mountainous")]
    Mountainous = 4     // Extreme elevation changes
}
