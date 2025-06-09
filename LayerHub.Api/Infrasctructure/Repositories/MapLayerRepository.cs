using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Core.Domain.Mapping;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;
using LayerHub.Shared.ReadDocuments;
using LayerHub.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace LayerHub.Api.Infrasctructure.Repositories;

public class MapLayerRepository : IMapLayerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly MongoDbContext _mongoDbContext;

    public MapLayerRepository(ApplicationDbContext context, MongoDbContext mongoDbContext)
    {
        _context = context;
        _mongoDbContext = mongoDbContext;
    }

    public async Task<PaginatedList<MapLayerDto>> Get(BasePaginator paginator, CancellationToken token)
    {
        // Get total count for pagination
        var totalCount = await _mongoDbContext.MapLayers
            .CountDocumentsAsync(Builders<MapLayerDocument>.Filter.Empty, cancellationToken: token);

        // Get paginated items
        var items = await _mongoDbContext.MapLayers
            .Find(Builders<MapLayerDocument>.Filter.Empty)
            .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
            .Limit(paginator.ItemsPerPage)
            .ToListAsync(token);

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)paginator.ItemsPerPage);

        // Map MongoDB documents to domain models if needed
        var mappedItems = items.Select(doc => MapLayerMapper.MapToDto(doc)).ToList();

        return new PaginatedList<MapLayerDto>(
            mappedItems,
            totalPages,
            (int)totalCount,
            paginator.Page,
            paginator.ItemsPerPage);
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
