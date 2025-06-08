namespace TrailFinder.Core.ValueObjects;

public record Coordinate
{
    public double Latitude { get; }
    public double Longitude { get; }

    public Coordinate(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude));
        
        if (longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude));

        Latitude = latitude;
        Longitude = longitude;
    }
}