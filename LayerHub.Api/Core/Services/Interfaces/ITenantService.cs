using LayerHub.Shared.Dto.Tenant;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services.Interfaces;

public interface ITenantService
{
    Task<PaginatedList<Tenant>> Get(BasePaginator paginator, CancellationToken token);
    Task<Tenant?> Get(Guid id);
    Task<Tenant> Create(NewTenantDto dto);
    Task<Tenant> Update(Guid id, UpdateTenantDto dto);
    Task Delete(Guid id);
}
