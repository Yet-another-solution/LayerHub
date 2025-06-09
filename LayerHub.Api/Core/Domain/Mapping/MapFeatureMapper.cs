using System.Text.Json;
using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Models;
using LayerHub.Shared.ReadDocuments;
using LayerHub.Shared.Utils;
using Riok.Mapperly.Abstractions;

namespace LayerHub.Api.Core.Domain.Mapping;

[Mapper]
public partial class MapFeatureMapper
{
    /// <summary>
    /// Maps from MapFeature entity to MapFeatureDto
    /// </summary>
    [MapperIgnoreSource(nameof(MapFeature.CreatedAt))]
    [MapperIgnoreSource(nameof(MapFeature.UpdatedAt))]
    [MapperIgnoreSource(nameof(MapFeature.DeletedAt))]
    [MapperIgnoreSource(nameof(MapFeature.OwnerId))]
    [MapperIgnoreSource(nameof(MapFeature.Owner))]
    public static partial MapFeatureDto MapToDto(MapFeature source);
    public static partial MapFeatureDto MapToDto(MapFeatureDocument source);
    
    /// <summary>
    /// Maps from NewMapFeatureDto to MapFeature entity
    /// </summary>
    [MapperIgnoreTarget(nameof(MapFeature.Id))]
    [MapperIgnoreTarget(nameof(MapFeature.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapFeature.UpdatedAt))]
    [MapperIgnoreTarget(nameof(MapFeature.DeletedAt))]
    [MapperIgnoreTarget(nameof(MapFeature.OwnerId))]
    [MapperIgnoreTarget(nameof(MapFeature.Owner))]
    public static partial MapFeature MapToEntity(NewMapFeatureDto source);
    
    /// <summary>
    /// Maps from a collection of MapFeature entities to a collection of MapFeatureDto
    /// </summary>
    public static partial List<MapFeatureDto> MapToDtoList(List<MapFeature> source);

    /// <summary>
    /// Maps a list of MapFeature entities to a list of MapFeatureDto objects.
    /// </summary>
    /// <param name="source">The list of MapFeature entities to map from.</param>
    /// <returns>A list of MapFeatureDto objects mapped from the given MapFeature entities.</returns>
    public static PaginatedList<MapFeatureDto> MapToPaginatedDtoList(PaginatedList<MapFeature> source)
    {
        var items = MapToDtoList(source.ToList());
        return new PaginatedList<MapFeatureDto>(
            items,
            source.TotalPages,
            source.TotalItems,
            source.PageIndex,
            source.ItemsPerPage);
    }

    /// <summary>
    /// Updates an existing MapFeature entity from a NewMapFeatureDto
    /// </summary>
    [MapperIgnoreTarget(nameof(MapFeature.Id))]
    [MapperIgnoreTarget(nameof(MapFeature.CreatedAt))]
    [MapperIgnoreTarget(nameof(MapFeature.UpdatedAt))]
    [MapperIgnoreTarget(nameof(MapFeature.DeletedAt))]
    [MapperIgnoreTarget(nameof(MapFeature.OwnerId))]
    [MapperIgnoreTarget(nameof(MapFeature.Owner))]
    public static partial void UpdateFromDto(UpdateMapFeatureDto source, MapFeature target);

    [MapperIgnoreSource(nameof(MapFeature.CreatedAt))]
    [MapperIgnoreSource(nameof(MapFeature.UpdatedAt))]
    [MapperIgnoreSource(nameof(MapFeature.DeletedAt))]
    [MapperIgnoreSource(nameof(MapFeature.OwnerId))]
    [MapperIgnoreSource(nameof(MapFeature.Owner))]
    public static partial MapFeatureDocument MapToDocument(MapFeature source);
}
