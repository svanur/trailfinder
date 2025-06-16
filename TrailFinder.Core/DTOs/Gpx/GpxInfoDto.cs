namespace TrailFinder.Core.DTOs.Gpx;

public record GpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    GeoPoint StartPoint
);

public record GeoPoint(
    double Latitude,
    double Longitude
);
