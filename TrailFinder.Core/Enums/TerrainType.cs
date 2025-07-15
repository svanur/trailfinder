using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum TerrainType
{
    [PgName("flat")]
    Flat,           // Minimal elevation changes
    
    [PgName("rolling")]
    Rolling,        // Moderate ups and downs
    
    [PgName("hilly")]
    Hilly,          // Significant elevation changes
    
    [PgName("mountainous")]
    Mountainous,     // Extreme elevation changes
    
    [PgName("unknown")]
    Unknown
}
