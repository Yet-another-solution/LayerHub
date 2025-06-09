namespace LayerHub.Shared.Models;

public class MapProjectLayer
{
    public Guid MapProjectId { get; set; }
    public MapProject? MapProject { get; set; }
    public Guid MapLayerId { get; set; }
    public MapLayer? MapLayer { get; set; }
}