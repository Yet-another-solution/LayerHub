using AutoMapper;
using LayerHub.Api.Core.Domain.Context;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Dto.Tenant;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services;

public class TenantService : ITenantService
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantRepository _tenantRepository;
    private readonly CurrentContext _currentContext;
    private readonly IMapper _mapper;

    public TenantService(ApplicationDbContext context, ITenantRepository tenantRepository,
        CurrentContext currentContext, IMapper mapper)
    {
        _context = context;
        _tenantRepository = tenantRepository;
        _currentContext = currentContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<Tenant>> Get(BasePaginator paginator, CancellationToken token)
    {
        return await _tenantRepository.Get(paginator, token);
    }

    public async Task<Tenant?> Get(Guid id)
    {
        var tenant = await _tenantRepository.Get(id);
        if (tenant is null)
        {
            throw new NotFoundException("Tenant not found");
        }

        return await _tenantRepository.Get(id);
    }

    public async Task<Tenant> Create(NewTenantDto dto)
    {
        var tenant = _mapper.Map<Tenant>(dto);

        _tenantRepository.Create(tenant);
        await _context.SaveChangesAsync();

        return tenant;
    }

    public async Task<Tenant> Update(Guid id, UpdateTenantDto dto)
    {
        var tenantToUpdate = await _tenantRepository.Get(id);

        if (tenantToUpdate is null)
        {
            throw new NotFoundException("Tenant not found");
        }

        tenantToUpdate = _mapper.Map(dto, tenantToUpdate);

        _tenantRepository.Update(tenantToUpdate);
        await _context.SaveChangesAsync();
        return tenantToUpdate;
    }

    public async Task Delete(Guid id)
    {
        var tenant = await _tenantRepository.Get(id);

        if (tenant is null)
        {
            throw new NotFoundException("Tenant not found");
        }

        _tenantRepository.Delete(tenant);
        await _context.SaveChangesAsync();
    }
}
