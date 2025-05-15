using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;

namespace LayerHub.Api.Application.Converter;

public class GeometryJsonConverter : JsonConverter<IGeometry>
{
    public override IGeometry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Save the current position
        var readerAtStart = reader;
        
        // Read to get the type property
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;
        
        if (!root.TryGetProperty("type", out var typeProperty))
            throw new JsonException("Geometry object doesn't contain a 'type' property");

        string geometryType = typeProperty.GetString() ?? throw new JsonException("Invalid geometry type");
        
        // Reset the reader
        reader = readerAtStart;
        
        // Deserialize to the correct concrete type based on the geometry type
        return geometryType switch
        {
            "Point" => JsonSerializer.Deserialize<PointGeometry>(ref reader, options),
            "Polygon" => JsonSerializer.Deserialize<PolygonGeometry>(ref reader, options),
            // Add other geometry types as needed
            _ => throw new JsonException($"Unsupported geometry type: {geometryType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, IGeometry value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}