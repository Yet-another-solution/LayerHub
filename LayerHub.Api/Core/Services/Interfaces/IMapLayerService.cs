using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services.Interfaces;

public interface IMapLayerService
{
    Task<PaginatedList<MapLayerDto>> Get(BasePaginator paginator, CancellationToken token);
    Task<MapLayer?> Get(Guid id);
    Task<MapLayer> Create(NewMapLayerDto dto);
    Task<MapLayer> Update(Guid id, UpdateMapLayerDto dto);
    Task Delete(Guid id);
}
