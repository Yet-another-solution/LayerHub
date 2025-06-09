using LayerHub.Shared.Dto.MapProject;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services.Interfaces;

public interface IMapProjectService
{
    Task<PaginatedList<MapProject>> Get(BasePaginator paginator, CancellationToken token);
    Task<MapProject?> Get(Guid id);
    Task<MapProject> Create(NewMapProjectDto dto);
    Task<MapProject> Update(Guid id, UpdateMapProjectDto dto);
    Task Delete(Guid id);
    
    // New methods for public access to published projects
    Task<MapProject?> GetPublished(Guid id);
}
