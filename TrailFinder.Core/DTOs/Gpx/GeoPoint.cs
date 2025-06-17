namespace TrailFinder.Core.DTOs.Gpx;

public record GeoPoint
{
    private const double EarthRadiusMeters = 6371e3;
    
    public double Latitude { get; }
    public double Longitude { get; }

    public GeoPoint(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public bool IsNearby(GeoPoint other, double thresholdMeters = 50)
    {
        const double radiansPerDegree = Math.PI / 180;
        
        var lat1Rad = Latitude * radiansPerDegree;
        var lon1Rad = Longitude * radiansPerDegree;
        var lat2Rad = other.Latitude * radiansPerDegree;
        var lon2Rad = other.Longitude * radiansPerDegree;

        var latDiff = lat2Rad - lat1Rad;
        var lonDiff = lon2Rad - lon1Rad;

        var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
        
        if (double.IsNaN(a) || a > 1)
        {
            return false;
        }

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusMeters * c;

        return !double.IsNaN(distance) && distance <= thresholdMeters;
    }
}
