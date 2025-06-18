using NetTopologySuite.Geometries;

namespace TrailFinder.Core.DTOs.Gpx;

public record TrailGpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    LineString RouteGeom

);

