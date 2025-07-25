using LayerHub.Shared.Models.Interfaces;

namespace LayerHub.Shared.Models;

public class MapProject : ISoftDeletable, IOwned, ITrackable
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public required string Url { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset VisibleStart { get; set; }
    public DateTimeOffset? VisibleEnd { get; set; }

    public List<MapProjectLayer> MapProjectLayers { get; set; } = new();

    // Interfaces
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid OwnerId { get; set; }
    public Tenant? Owner { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}