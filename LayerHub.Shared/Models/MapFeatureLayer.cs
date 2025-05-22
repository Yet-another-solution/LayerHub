namespace LayerHub.Shared.Models;

public class MapFeatureLayer
{
    public Guid MapFeatureId { get; set; }
    public MapFeature? MapFeature { get; set; }
    public Guid MapLayerId { get; set; }
    public MapLayer? MapLayer { get; set; }
}