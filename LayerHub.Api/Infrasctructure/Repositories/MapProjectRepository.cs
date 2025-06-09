using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace LayerHub.Api.Infrasctructure.Repositories;

public class MapProjectRepository : IMapProjectRepository
{
    private readonly ApplicationDbContext _context;

    public MapProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MapProject>> Get(BasePaginator paginator, CancellationToken token)
    {
        var query = _context.MapProjects
            .AsQueryable();

        return await PaginatedList<MapProject>.CreateAsync(query, paginator.Page, paginator.ItemsPerPage, token);
    }

    public async Task<MapProject?> Get(Guid id)
    {
        return await _context.MapProjects
            .Include(mp => mp.MapProjectLayers)
            .ThenInclude(mpl => mpl.MapLayer)
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public void Create(MapProject mapProject)
    {
        _context.MapProjects.Add(mapProject);
    }

    public void Update(MapProject mapProject)
    {
        _context.MapProjects.Update(mapProject);
    }

    public void Delete(MapProject mapProject)
    {
        _context.MapProjects.Remove(mapProject);
    }
}
