using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum RouteType
{
    [PgName("circular")]
    Circular,
    
    [PgName("out-and-back")]
    OutAndBack,
    
    [PgName("point-to-point")]
    PointToPoint,
    
    [PgName("unknown")]
    Unknown
}
