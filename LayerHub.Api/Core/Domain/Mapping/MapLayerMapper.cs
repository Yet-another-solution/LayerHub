using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;
using LayerHub.Shared.ReadDocuments;
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

    [MapperIgnoreSource(nameof(MapLayerDocument.OwnerId))]
    [MapperIgnoreTarget(nameof(MapLayerDto.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapLayerDto.UpdatedAt))]
    public static partial MapLayerDto MapToDto(MapLayerDocument document);

    /// <summary>
    /// Maps MapLayer to MapLayerDto including Features
    /// </summary>
    public static MapLayerDto MapToDtoWithFeatures(MapLayer source)
    {
        var dto = MapToDto(source);

        dto.MapFeatures = source.MapFeatureLayers
            .Where(mfl => mfl.MapFeature != null)
            .Select(mfl => MapFeatureMapper.MapToDto(mfl.MapFeature!))
            .ToList();

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

    [MapperIgnoreTarget(nameof(MapLayerDocument.OwnerId))]
    [MapperIgnoreSource(nameof(MapLayer.CreatedAt))]
    [MapperIgnoreSource(nameof(MapLayer.UpdatedAt))]
    [MapperIgnoreSource(nameof(MapLayer.DeletedAt))]
    [MapperIgnoreSource(nameof(MapLayer.MapFeatureLayers))]
    [MapperIgnoreSource(nameof(MapLayer.OwnerId))]
    [MapperIgnoreSource(nameof(MapLayer.Owner))]
    [MapperIgnoreTarget(nameof(MapLayerDocument.MapFeatures))]
    public static partial MapLayerDocument MapToDocument(MapLayer source);

    public static MapLayerDocument MapToDocumentWithFeatures(MapLayer source)
    {
        var document = MapToDocument(source);
        
        document.MapFeatures = source.MapFeatureLayers
            .Where(mfl => mfl.MapFeature != null)
            .Select(mfl => MapFeatureMapper.MapToDocument(mfl.MapFeature!))
            .ToList();
        
        return document;
    }
}