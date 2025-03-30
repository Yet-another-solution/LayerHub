using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace LayerHub.Api.Infrasctructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly ApplicationDbContext _context;

    public TenantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Tenant>> Get(BasePaginator paginator, CancellationToken token)
    {
        var query = _context.Tenants
            .AsQueryable();

        return await PaginatedList<Tenant>.CreateAsync(query, paginator.Page, paginator.ItemsPerPage, token);
    }

    public async Task<Tenant?> Get(Guid id)
    {
        var tenant = _context.Tenants
            .Include(t => t.Users)
            .SingleOrDefaultAsync(t => t.Id == id);
        return await tenant;
    }

    public void Create(Tenant tenant)
    {
        _context.Tenants.Add(tenant);
    }

    public void Update(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
    }

    public void Delete(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
    }
}