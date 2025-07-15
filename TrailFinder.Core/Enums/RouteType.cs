namespace TrailFinder.Core.Enums;

public enum RouteType
{
    Circular,      // Start and end points are close to each other, forming a loop
    OutAndBack,    // Trail follows roughly the same path out and back
    PointToPoint,  // Start and end points are significantly different (A to B)
    Unknown        // Cannot determine the type
}