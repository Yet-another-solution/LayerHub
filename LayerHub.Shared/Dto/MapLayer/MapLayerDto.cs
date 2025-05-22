using System.Text.Json;
using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Models;

namespace LayerHub.Shared.Dto.MapLayer;

public class MapLayerDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public JsonDocument? Layer { get; set; }
    public List<MapFeatureDto> MapFeatures { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
