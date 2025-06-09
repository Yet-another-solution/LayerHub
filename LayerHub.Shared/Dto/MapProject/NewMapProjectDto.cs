namespace LayerHub.Shared.Dto.MapProject;

public class NewMapProjectDto
{
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public required string Url { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset VisibleStart { get; set; }
    public DateTimeOffset? VisibleEnd { get; set; }
    public List<Guid> LayerIds { get; set; } = new();
}
