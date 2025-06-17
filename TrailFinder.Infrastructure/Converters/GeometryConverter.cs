using NetTopologySuite.Geometries;

namespace TrailFinder.Infrastructure.Converters;

public static class GeometryConverter
{
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326);
    
    public static Point? ToPoint(object? supabaseGeometry)
    {
        if (supabaseGeometry == null) return null;
        
        // Supabase returns GeoJSON format
        var coordinates = ParseGeoJsonCoordinates(supabaseGeometry);
        if (coordinates == null || coordinates.Length < 2) return null;
        
        return GeometryFactory.CreatePoint(new Coordinate(
            coordinates[0], // longitude
            coordinates[1]  // latitude
        ));
    }

    public static LineString? ToLineString(object? supabaseGeometry)
    {
        if (supabaseGeometry == null) return null;

        try
        {
            // Parse the GeoJSON coordinates array
            var coordinatesList = ParseGeoJsonLineString(supabaseGeometry);
            if (coordinatesList == null || !coordinatesList.Any()) return null;

            var coordinates = coordinatesList
                .Select(coord => new Coordinate(coord[0], coord[1]))
                .ToArray();

            return GeometryFactory.CreateLineString(coordinates);
        }
        catch
        {
            return null;
        }
    }

    public static object ToGeoJson(Point point)
    {
        return new
        {
            type = "Point",
            coordinates = new[] { point.X, point.Y }
        };
    }

    public static object ToGeoJson(LineString lineString)
    {
        return new
        {
            type = "LineString",
            coordinates = lineString.Coordinates.Select(c => new[] { c.X, c.Y }).ToArray()
        };
    }

    private static double[]? ParseGeoJsonCoordinates(object geometry)
    {
        // Handle various GeoJSON formats that Supabase might return
        if (geometry is System.Text.Json.JsonElement jsonElement)
        {
            if (jsonElement.TryGetProperty("coordinates", out var coordinates))
            {
                return coordinates.EnumerateArray()
                    .Select(x => x.GetDouble())
                    .ToArray();
            }
        }
        return null;
    }

    private static List<double[]>? ParseGeoJsonLineString(object geometry)
    {
        if (geometry is System.Text.Json.JsonElement jsonElement)
        {
            if (jsonElement.TryGetProperty("coordinates", out var coordinates))
            {
                return coordinates.EnumerateArray()
                    .Select(point => point.EnumerateArray()
                        .Select(x => x.GetDouble())
                        .ToArray())
                    .ToList();
            }
        }
        return null;
    }
}