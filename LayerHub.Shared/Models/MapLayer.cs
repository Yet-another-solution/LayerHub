using System.Text.Json;
using Community.Blazor.MapLibre.Models.Layers;
using LayerHub.Shared.Models.Interfaces;

namespace LayerHub.Shared.Models;

public class MapLayer : ISoftDeletable, IOwned, ITrackable
{
    /// <summary>
    /// Gets or sets the unique identifier for the map layer.
    /// </summary>
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
    public JsonDocument? Layer { get; set; }

    /// <summary>
    /// Gets or sets the collection of feature layers associated with the map layer.
    /// </summary>
    public List<MapFeatureLayer> MapFeatureLayers { get; set; } = new();

    // Interfaces
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid OwnerId { get; set; }
    public Tenant? Owner { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}