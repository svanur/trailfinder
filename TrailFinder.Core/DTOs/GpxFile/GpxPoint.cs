// TrailFinder.Core\DTOs\GpxFile\GpxPoint.cs
using System.Xml.Linq;
using NetTopologySuite.Geometries;

namespace TrailFinder.Core.DTOs.GpxFile;

public readonly record struct GpxPoint
{
    private const double EarthRadiusMeters = 6371e3;

    public double Latitude { get; }
    public double Longitude { get; }
    public double Elevation { get; }

    public GpxPoint(double latitude, double longitude, double elevation, bool isE7Format = false)
    {
        // If the values are in E7 format (like 642090250), convert them to decimal degrees
        if (isE7Format)
        {
            Latitude = latitude * 1e-7;
            Longitude = longitude * 1e-7;
        }
        else
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        Elevation = elevation;
    }

    public GpxPoint(Point point)
    {
        Latitude = point.Coordinate.Y;
        Longitude = point.Coordinate.X;
        Elevation = point.Coordinate.Z;
    }

    public static GpxPoint FromXElement(XElement point, XNamespace ns)
    {
        // GPX files use decimal degrees, so no E7 conversion needed
        var lat = ParseDoubleAttribute(point, "lat");
        var lon = ParseDoubleAttribute(point, "lon");
        var ele = ParseElevation(point, ns);

        return new GpxPoint(lat, lon, ele, true);
    }

    private static double ParseDoubleAttribute(XElement element, string attributeName)
    {
        var value = element.Attribute(attributeName)?.Value;
        return value != null ? double.Parse(value) : 0;
    }

    private static double ParseElevation(XElement point, XNamespace ns)
    {
        var eleElement = point.Element(ns + "ele");
        if (eleElement == null)
        {
            throw new  Exception("No ele element found"); //TODO: ?
        }
        
        var elevationInMeters = double.Parse(eleElement.Value);
        return elevationInMeters;
    }

    /// <summary>
    /// Determines whether the current <c>GpxPoint</c> is within a specified threshold distance from another <c>GpxPoint</c>.
    /// </summary>
    /// <param name="that">The <c>GpxPoint</c> to compare the current point with.</param>
    /// <param name="thresholdMeters">The distance threshold in meters. Defaults to 50 meters.</param>
    /// <returns>
    /// <c>true</c> if the current point is within the specified threshold distance from the that point; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNearby(GpxPoint that, double thresholdMeters = 50)
    {
        const double radiansPerDegree = Math.PI / 180;

        var lat1Rad = (double)(Latitude * radiansPerDegree);
        var lon1Rad = Longitude * radiansPerDegree;
        var lat2Rad = (double)(that.Latitude * radiansPerDegree);
        var lon2Rad = that.Longitude * radiansPerDegree;

        var latDiff = lat2Rad - lat1Rad;
        var lonDiff = lon2Rad - lon1Rad;

        var a = Math.Sin((double)(latDiff / 2)) * Math.Sin((double)(latDiff / 2)) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin((double)(lonDiff / 2)) * Math.Sin((double)(lonDiff / 2));
        
        if (double.IsNaN(a) || a > 1)
        {
            return false;
        }

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusMeters * c;

        return !double.IsNaN(distance) && distance <= thresholdMeters;
    }

    public double CalculateDistance(GpxPoint that)
    {
        const double radiansPerDegree = Math.PI / 180;
    
        // Check for valid coordinate ranges
        if (!IsValidCoordinate(this) || !IsValidCoordinate(that))
        {
            return 0;
        }
    
        var lat1Rad = Latitude * radiansPerDegree;
        var lon1Rad = Longitude * radiansPerDegree;
        var lat2Rad = that.Latitude * radiansPerDegree;
        var lon2Rad = that.Longitude * radiansPerDegree;

        var latDiff = lat2Rad - lat1Rad;
        var lonDiff = lon2Rad - lon1Rad;

        var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
            
        // Check if 'a' is valid for further calculation
        if (double.IsNaN(a) || a > 1)
        {
            return 0;
        }

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusMeters * c;

        return double.IsNaN(distance) 
            ? 0 
            : distance;
    }

    private static bool IsValidCoordinate(GpxPoint point)
    {
        return point.Latitude is >= -90 and <= 90 &&
               point.Longitude is >= -180 and <= 180 &&
               !double.IsNaN(point.Latitude) && !double.IsNaN(point.Longitude);
    }
}
