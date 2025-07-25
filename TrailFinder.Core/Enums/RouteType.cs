using System.Runtime.Serialization;
using NpgsqlTypes;

namespace TrailFinder.Core.Enums;

public enum RouteType
{
    [PgName("unknown")]
    Unknown = 0,
        
    [PgName("circular")]
    Circular = 1,
    
    [PgName("outAndBack")]
    OutAndBack = 2,
    
    [PgName("pointToPoint")]
    PointToPoint = 3,
    
    
}