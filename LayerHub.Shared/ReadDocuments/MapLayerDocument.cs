using System.Text.Json;
using LayerHub.Shared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LayerHub.Shared.ReadDocuments;

public class MapLayerDocument
{
    /// <summary>
    /// Gets or sets the unique identifier for the map layer.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the map layer.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the descriptive text for the map layer.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the serialized representation of the map layer.
    /// </summary>
    public JsonDocument Layer { get; set; } = JsonDocument.Parse("{}");

    /// <summary>
    /// Gets or sets the collection of feature layers associated with the map layer.
    /// </summary>
    public List<MapFeatureDocument> MapFeatures { get; set; } = new();

    // Interfaces
    [BsonRepresentation(BsonType.String)]
    public Guid OwnerId { get; set; }
}