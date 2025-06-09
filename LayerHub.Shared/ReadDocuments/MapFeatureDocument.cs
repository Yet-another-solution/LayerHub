using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LayerHub.Shared.ReadDocuments;

public class MapFeatureDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public required JsonDocument GeometryJson { get; set; }

    public int? Size { get; set; }
    public JsonDocument? AdditionalParameters { get; set; }
}