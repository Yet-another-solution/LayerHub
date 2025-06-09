using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;

namespace LayerHub.Shared.Dto.MapProject;

public class MapProjectDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public required string Url { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset VisibleStart { get; set; }
    public DateTimeOffset? VisibleEnd { get; set; }
    public List<MapLayerDto> Layers { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
