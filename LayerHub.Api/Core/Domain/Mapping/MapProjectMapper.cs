using LayerHub.Shared.Dto.MapProject;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Riok.Mapperly.Abstractions;

namespace LayerHub.Api.Core.Domain.Mapping;

[Mapper]
public partial class MapProjectMapper
{
    /// <summary>
    /// Maps from MapProject entity to MapProjectDto
    /// </summary>
    [MapperIgnoreSource(nameof(MapProject.DeletedAt))]
    [MapperIgnoreSource(nameof(MapProject.OwnerId))]
    [MapperIgnoreSource(nameof(MapProject.Owner))]
    [MapperIgnoreSource(nameof(MapProject.MapProjectLayers))]
    [MapperIgnoreTarget(nameof(MapProjectDto.Layers))]
    public static partial MapProjectDto MapToDto(MapProject source);
    
    /// <summary>
    /// Maps from NewMapProjectDto to MapProject entity
    /// </summary>
    [MapperIgnoreTarget(nameof(MapProject.Id))]
    [MapperIgnoreTarget(nameof(MapProject.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapProject.UpdatedAt))]
    [MapperIgnoreTarget(nameof(MapProject.DeletedAt))]
    [MapperIgnoreTarget(nameof(MapProject.OwnerId))]
    [MapperIgnoreTarget(nameof(MapProject.Owner))]
    [MapperIgnoreTarget(nameof(MapProject.MapProjectLayers))]
    [MapperIgnoreSource(nameof(NewMapProjectDto.LayerIds))]
    public static partial MapProject MapToEntity(NewMapProjectDto source);
    
    /// <summary>
    /// Maps from a collection of MapProject entities to a collection of MapProjectDto
    /// </summary>
    public static partial List<MapProjectDto> MapToDtoList(List<MapProject> source);
    
    /// <summary>
    /// Maps a MapProject entity to a MapProjectDto with included layers
    /// </summary>
    public static MapProjectDto MapToDtoWithLayers(MapProject source)
    {
        var dto = MapToDto(source);

        foreach (var projectLayer in source.MapProjectLayers)
        {
            if (projectLayer.MapLayer != null)
            {
                dto.Layers.Add(MapLayerMapper.MapToDto(projectLayer.MapLayer));
            }
        }

        return dto;
    }

    /// <summary>
    /// Maps a list of MapProject entities to a list of MapProjectDto objects.
    /// </summary>
    /// <param name="source">The list of MapProject entities to map from.</param>
    /// <returns>A list of MapProjectDto objects mapped from the given MapProject entities.</returns>
    public static PaginatedList<MapProjectDto> MapToPaginatedDtoList(PaginatedList<MapProject> source)
    {
        var items = MapToDtoList(source.ToList());
        return new PaginatedList<MapProjectDto>(
            items,
            source.TotalPages,
            source.TotalItems,
            source.PageIndex,
            source.ItemsPerPage);
    }

    /// <summary>
    /// Updates an existing MapProject entity from an UpdateMapProjectDto
    /// </summary>
    [MapperIgnoreTarget(nameof(MapProject.Id))]
    [MapperIgnoreTarget(nameof(MapProject.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapProject.UpdatedAt))]
    [MapperIgnoreTarget(nameof(MapProject.DeletedAt))]
    [MapperIgnoreTarget(nameof(MapProject.OwnerId))]
    [MapperIgnoreTarget(nameof(MapProject.Owner))]
    [MapperIgnoreTarget(nameof(MapProject.MapProjectLayers))]
    [MapperIgnoreSource(nameof(UpdateMapProjectDto.LayerIds))]
    public static partial void UpdateFromDto(UpdateMapProjectDto source, MapProject target);
}
