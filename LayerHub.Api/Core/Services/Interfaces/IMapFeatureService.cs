using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services.Interfaces;

public interface IMapFeatureService
{
    Task<PaginatedList<MapFeature>> Get(BasePaginator paginator, CancellationToken token);
    Task<MapFeature?> Get(Guid id);
    Task<MapFeature> Create(NewMapFeatureDto dto);
    Task<MapFeature> Update(Guid id, UpdateMapFeatureDto dto);
    Task Delete(Guid id);
}