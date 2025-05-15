using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;

namespace LayerHub.Shared.Dto.MapFeature;

public class NewMapFeatureDto
{
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public JsonDocument? GeometryJson { get; set; }

    public int? Size { get; set; }
    public JsonDocument? AdditionalParameters { get; set; }
}