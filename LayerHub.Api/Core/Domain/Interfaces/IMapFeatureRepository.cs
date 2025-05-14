using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Domain.Interfaces;

public interface IMapFeatureRepository
{
    Task<PaginatedList<MapFeature>> Get(BasePaginator paginator, CancellationToken token);
    Task<MapFeature?> Get(Guid id);
    void Create(MapFeature mapFeature);
    void Update(MapFeature mapFeature);
    void Delete(MapFeature mapFeature);
}