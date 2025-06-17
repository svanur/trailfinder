namespace TrailFinder.Core.DTOs.Gpx;

public record GpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    GeoPoint StartPoint,
    GeoPoint EndPoint
);

public record GeoPoint(
    double Latitude,
    double Longitude
);
