using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum RouteType
{
    [PgName("unknown")]
    Unknown = 0,
        
    [PgName("circular")]
    Circular = 1,
    
    [PgName("out-and-back")]
    OutAndBack = 2,
    
    [PgName("point-to-point")]
    PointToPoint = 3,
    
    
}
