using System.Text.Json;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace TrailFinder.Api.Converters;

public class LineStringConverter : JsonConverter<LineString>
{
    private readonly GeometryFactory _geometryFactory = new(
        new PrecisionModel(PrecisionModels.Floating), 
        4326
    );

    public override LineString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected start of object");

        List<Coordinate> coordinates = new();
        bool foundCoordinates = false;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString()!;
                reader.Read();

                if (propertyName == "coordinates" && reader.TokenType == JsonTokenType.StartArray)
                {
                    foundCoordinates = true;
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            reader.Read();
                            var x = reader.GetDouble(); // longitude
                            reader.Read();
                            var y = reader.GetDouble(); // latitude
                            
                            // Handle optional Z coordinate
                            reader.Read();
                            var z = 0.0;
                            if (reader.TokenType == JsonTokenType.Number)
                            {
                                z = reader.GetDouble();
                                reader.Read(); // Move past the end array
                            }
                            
                            coordinates.Add(new CoordinateZ(x, y, z));
                        }
                    }
                }
            }
        }

        if (!foundCoordinates)
            throw new JsonException("No coordinates found in LineString");

        return coordinates.Count == 0 ? null : _geometryFactory.CreateLineString(coordinates.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, LineString? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", "LineString");
        writer.WriteStartArray("coordinates");
        
        foreach (var coordinate in value.Coordinates)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(coordinate.X);
            writer.WriteNumberValue(coordinate.Y);
            if (!double.IsNaN(coordinate.Z) && !double.IsInfinity(coordinate.Z))
            {
                writer.WriteNumberValue(coordinate.Z);
            }
            writer.WriteEndArray();
        }
        
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}