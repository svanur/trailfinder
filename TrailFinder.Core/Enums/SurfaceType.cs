using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum SurfaceType
{
    [PgName("unknown")]
    Unknown = 0,
    
    [PgName("trail")]
    Trail = 1,
    
    [PgName("asphalt")]
    Asphalt = 2,
    
    [PgName("sand")]
    Sand = 3,
    
    [PgName("snow")]
    Snow = 4,
    
    [PgName("ice")]
    Ice = 5
}
