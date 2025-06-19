using System.Text.Json;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace TrailFinder.Api.Converters;

public class LineStringConverter : JsonConverter<LineString>
{
    private readonly GeometryFactory _geometryFactory = new();

    public override LineString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected start of array");

        var coordinates = new List<Coordinate>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected start of coordinate array");

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
                throw new JsonException("Expected number for X coordinate");
            var x = reader.GetDouble();

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
                throw new JsonException("Expected number for Y coordinate");
            var y = reader.GetDouble();

            // Modified Z coordinate handling
            reader.Read();
            var z = 0.0;
            if (reader.TokenType == JsonTokenType.Number)
            {
                z = reader.GetDouble();
            }

            reader.Read(); // Read past the end of coordinate array

            coordinates.Add(new CoordinateZ(x, y, z));
        }

        return coordinates.Count == 0 ? null : _geometryFactory.CreateLineString(coordinates.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, LineString? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        foreach (var coordinate in value.Coordinates)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(coordinate.X);
            writer.WriteNumberValue(coordinate.Y);
            // Handle potentially invalid Z values
            var z = double.IsInfinity(coordinate.Z) || double.IsNaN(coordinate.Z) ? 0 : coordinate.Z;
            writer.WriteNumberValue(z);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
}
