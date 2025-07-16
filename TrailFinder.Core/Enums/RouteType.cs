using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum RouteType
{
    [PgName("circular")]
    Circular = 0,
    
    [PgName("out-and-back")]
    OutAndBack = 1,
    
    [PgName("point-to-point")]
    PointToPoint = 2,
    
    [PgName("unknown")]
    Unknown = 3
}
