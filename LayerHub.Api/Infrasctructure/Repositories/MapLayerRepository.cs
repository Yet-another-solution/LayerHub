using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace LayerHub.Api.Infrasctructure.Repositories;

public class MapLayerRepository : IMapLayerRepository
{
    private readonly ApplicationDbContext _context;

    public MapLayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MapLayer>> Get(BasePaginator paginator, CancellationToken token)
    {
        var query = _context.MapLayers
            .AsQueryable();

        return await PaginatedList<MapLayer>.CreateAsync(query, paginator.Page, paginator.ItemsPerPage, token);
    }

    public async Task<MapLayer?> Get(Guid id)
    {
        return await _context.MapLayers
            .Include(ml => ml.MapFeatureLayers)
            .ThenInclude(mfl => mfl.MapFeature)
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public void Create(MapLayer mapLayer)
    {
        _context.MapLayers.Add(mapLayer);
    }

    public void Update(MapLayer mapLayer)
    {
        _context.MapLayers.Update(mapLayer);
    }

    public void Delete(MapLayer mapLayer)
    {
        _context.MapLayers.Remove(mapLayer);
    }
}
