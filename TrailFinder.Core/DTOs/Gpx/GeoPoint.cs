using System.Xml.Linq;

namespace TrailFinder.Core.DTOs.Gpx;

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
    /// 
    /// </summary>
    /// <example>
    /// var gpxInfo = await ExtractGpxInfo(gpxStream);
    /// bool isCircularTrail = gpxInfo.StartPoint.IsNearby(gpxInfo.EndPoint);
    /// </example>
    /// <param name="other"></param>
    /// <param name="thresholdMeters"></param>
    /// <returns></returns>
    public bool IsNearby(GpxPoint other, double thresholdMeters = 50)
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
