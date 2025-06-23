using NetTopologySuite.Geometries;

namespace TrailFinder.Core.DTOs.Gpx.Responses;

public record GpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    LineString RouteGeom
);

