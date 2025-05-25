using System.Text.Json;
using LayerHub.Shared.Models;

namespace LayerHub.Shared.Dto.MapLayer;

public class NewMapLayerDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public JsonDocument? Layer { get; set; }
    public List<Guid> MapFeatureIds { get; set; } = new();
}
