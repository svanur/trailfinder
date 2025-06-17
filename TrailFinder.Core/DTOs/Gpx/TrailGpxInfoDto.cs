namespace TrailFinder.Core.DTOs.Gpx;

public record TrailGpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    GeoPoint StartPoint,
    GeoPoint EndPoint
);

