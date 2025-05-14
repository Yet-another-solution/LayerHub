using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;
using LayerHub.Shared.Models.Interfaces;

namespace LayerHub.Shared.Models;

public class MapFeature : ITrackable, IOwned, ISoftDeletable
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public required JsonDocument GeometryJson { get; set; }
    // This is the property for application code - not stored in database
    [NotMapped]
    [JsonIgnore]
    public IGeometry? Geometry
    {
        get
        {
            try
            {
                return JsonSerializer.Deserialize<IGeometry>(GeometryJson);
            }
            catch
            {
                return null;
            }
        }
        set
        {
            if (value != null)
            {
                string json = JsonSerializer.Serialize(value);
                GeometryJson = JsonDocument.Parse(json);
            }
            else
            {
                GeometryJson = JsonDocument.Parse("{}");
            }

        }
    }

    public int? Size { get; set; }
    public JsonDocument? AdditionalParameters { get; set; }

    // Interfaces
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid OwnerId { get; set; }
    public Tenant? Owner { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}