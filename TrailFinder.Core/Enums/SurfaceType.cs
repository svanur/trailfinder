using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum SurfaceType
{
    [PgName("unknown")]
    Unknown = 0,
    
    [PgName("trail")]
    Trail = 1,
    
    [PgName("paved")]
    Paved = 2,
    
    [PgName("mixed")]
    Mixed = 3
}
