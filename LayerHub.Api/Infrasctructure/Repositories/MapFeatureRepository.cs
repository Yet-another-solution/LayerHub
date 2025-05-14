using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace LayerHub.Api.Infrasctructure.Repositories;

public class MapFeatureRepository : IMapFeatureRepository
{
    private readonly ApplicationDbContext _context;

    public MapFeatureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MapFeature>> Get(BasePaginator paginator, CancellationToken token)
    {
        var query = _context.MapFeatures
            .AsQueryable();

        return await PaginatedList<MapFeature>.CreateAsync(query, paginator.Page, paginator.ItemsPerPage, token);
    }

    public async Task<MapFeature?> Get(Guid id)
    {
        return await _context.MapFeatures
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    
    public void Create(MapFeature mapFeature)
    {
        _context.MapFeatures.Add(mapFeature);
    }

    public void Update(MapFeature mapFeature)
    {
        _context.MapFeatures.Update(mapFeature);
    }

    public void Delete(MapFeature mapFeature)
    {
        _context.MapFeatures.Remove(mapFeature);
    }
}