using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Domain.Interfaces;

public interface IMapProjectRepository
{
    Task<PaginatedList<MapProject>> Get(BasePaginator paginator, CancellationToken token);
    Task<MapProject?> Get(Guid id);
    void Create(MapProject mapProject);
    void Update(MapProject mapProject);
    void Delete(MapProject mapProject);
}
