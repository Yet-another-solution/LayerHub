using System.Text.Json;
using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Riok.Mapperly.Abstractions;

namespace LayerHub.Api.Core.Domain.Mapping;

[Mapper]
public partial class MapLayerMapper
{
    /// <summary>
    /// Maps from MapLayer entity to MapLayerDto
    /// </summary>
    [MapperIgnoreSource(nameof(MapLayer.MapFeatureLayers))]
    [MapperIgnoreSource(nameof(MapLayer.DeletedAt))]
    [MapperIgnoreSource(nameof(MapLayer.OwnerId))]
    [MapperIgnoreSource(nameof(MapLayer.Owner))]
    [MapperIgnoreTarget(nameof(MapLayerDto.MapFeatures))]
    [UserMapping(Default = true)]
    public static partial MapLayerDto MapToDto(MapLayer source);
    
    /// <summary>
    /// Maps MapFeatureLayers collection to associated MapFeatureDto list
    /// </summary>
    [MapperIgnoreTarget(nameof(MapLayerDto.MapFeatures))]
    private static List<MapFeatureDto> MapFeatureLayersToFeatureDtos(ICollection<MapFeatureLayer> mapFeatureLayers)
    {
        return mapFeatureLayers
            .Where(mfl => mfl.MapFeature != null)
            .Select(mfl => MapFeatureMapper.MapToDto(mfl.MapFeature!))
            .ToList();
    }

    /// <summary>
    /// Maps MapLayer to MapLayerDto including Features
    /// </summary>
    public static MapLayerDto MapToDtoWithFeatures(MapLayer source)
    {
        var dto = MapToDto(source);

        dto.MapFeatures = MapFeatureLayersToFeatureDtos(source.MapFeatureLayers);

        return dto;
    }

    
    /// <summary>
    /// Maps from NewMapLayerDto to MapLayer entity
    /// </summary>
    [MapperIgnoreTarget(nameof(MapLayer.Id))]
    [MapperIgnoreTarget(nameof(MapLayer.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapLayer.UpdatedAt))]
    [MapperIgnoreTarget(nameof(MapLayer.DeletedAt))]
    [MapperIgnoreTarget(nameof(MapLayer.OwnerId))]
    [MapperIgnoreTarget(nameof(MapLayer.Owner))]
    [MapperIgnoreTarget(nameof(MapLayer.MapFeatureLayers))]
    [MapperIgnoreSource(nameof(NewMapLayerDto.MapFeatureIds))]
    public static partial MapLayer MapToEntity(NewMapLayerDto source);

    /// <summary>
    /// Maps from a collection of MapLayer entities to a collection of MapLayerDto
    /// </summary>
    public static partial List<MapLayerDto> MapToDtoList(List<MapLayer> source);

    /// <summary>
    /// Maps a PaginatedList of MapLayer entities to a PaginatedList of MapLayerDto objects.
    /// </summary>
    /// <param name="source">The paginated list of MapLayer entities to map from.</param>
    /// <returns>A paginated list of MapLayerDto objects.</returns>
    public static PaginatedList<MapLayerDto> MapToPaginatedDtoList(PaginatedList<MapLayer> source)
    {
        var items = MapToDtoList(source.ToList());
        return new PaginatedList<MapLayerDto>(
            items,
            source.TotalPages,
            source.TotalItems,
            source.PageIndex,
            source.ItemsPerPage);
    }
    
    /// <summary>
    /// Updates an existing MapLayer entity from an UpdateMapLayerDto
    /// </summary>
    [MapperIgnoreTarget(nameof(MapLayer.Id))]
    [MapperIgnoreTarget(nameof(MapLayer.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapLayer.UpdatedAt))]
    [MapperIgnoreTarget(nameof(MapLayer.DeletedAt))]
    [MapperIgnoreTarget(nameof(MapLayer.OwnerId))]
    [MapperIgnoreTarget(nameof(MapLayer.Owner))]
    [MapperIgnoreTarget(nameof(MapLayer.MapFeatureLayers))]
    [MapperIgnoreSource(nameof(UpdateMapLayerDto.MapFeatureIds))]
    public static partial void UpdateFromDto(UpdateMapLayerDto source, MapLayer target);
}