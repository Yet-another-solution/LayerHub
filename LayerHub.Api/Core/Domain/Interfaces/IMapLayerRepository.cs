using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Domain.Interfaces;

public interface IMapLayerRepository
{
    Task<PaginatedList<MapLayerDto>> Get(BasePaginator paginator, CancellationToken token);
    Task<MapLayer?> Get(Guid id);
    void Create(MapLayer mapLayer);
    void Update(MapLayer mapLayer);
    void Delete(MapLayer mapLayer);
}
