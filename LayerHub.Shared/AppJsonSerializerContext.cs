using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.MapFeature;

namespace LayerHub.Shared;

// Add this to a shared library accessible by both API and Web projects
[JsonSerializable(typeof(IGeometry))]
[JsonSerializable(typeof(PointGeometry))]
[JsonSerializable(typeof(LineGeometry))]
[JsonSerializable(typeof(PolygonGeometry))]
[JsonSerializable(typeof(MultiPointGeometry))]
[JsonSerializable(typeof(MultiLineGeometry))]
[JsonSerializable(typeof(MultiPolygonGeometry))]
[JsonSerializable(typeof(MapFeatureDto))]
[JsonSerializable(typeof(PaginatedListDto<MapFeatureDto>))]
// Add other types that need serialization
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}