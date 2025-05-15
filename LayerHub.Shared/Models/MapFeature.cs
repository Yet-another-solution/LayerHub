using System.Text.Json;
using LayerHub.Shared.Models.Interfaces;

namespace LayerHub.Shared.Models;

public class MapFeature : ITrackable, IOwned, ISoftDeletable
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public required JsonDocument GeometryJson { get; set; }

    public int? Size { get; set; }
    public JsonDocument? AdditionalParameters { get; set; }

    // Interfaces
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid OwnerId { get; set; }
    public Tenant? Owner { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}