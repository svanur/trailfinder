using System.Xml.Linq;

namespace TrailFinder.Core.DTOs.Gpx;

public record struct GpxPoint
{
    private const double E7ToDecimal = 1e-7;
    public double Latitude { get; }
    public double Longitude { get; }
    public double? Elevation { get; }

    public GpxPoint(double latitude, double longitude, double? elevation = null)
    {
        // Convert from E7 format to decimal degrees
        Latitude = latitude * E7ToDecimal;
        Longitude = longitude * E7ToDecimal;
        Elevation = elevation;
    }

    public static GpxPoint FromXElement(XElement point, XNamespace ns)
    {
        var lat = ParseDoubleAttribute(point, "lat");
        var lon = ParseDoubleAttribute(point, "lon");
        var ele = ParseElevation(point, ns);

        return new GpxPoint(lat, lon, ele);
    }

    private static double ParseDoubleAttribute(XElement element, string attributeName)
    {
        var value = element.Attribute(attributeName)?.Value;
        return value != null ? double.Parse(value) : 0;
    }

    private static double? ParseElevation(XElement point, XNamespace ns)
    {
        var eleElement = point.Element(ns + "ele");
        return eleElement != null ? double.Parse(eleElement.Value) : null;
    }
}
