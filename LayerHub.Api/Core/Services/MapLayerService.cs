using AutoMapper;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Core.Domain.Mapping;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services;

public class MapLayerService : IMapLayerService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapLayerRepository _mapLayerRepository;

    public MapLayerService(ApplicationDbContext context, IMapLayerRepository mapLayerRepository)
    {
        _context = context;
        _mapLayerRepository = mapLayerRepository;
    }

    public async Task<PaginatedList<MapLayer>> Get(BasePaginator paginator, CancellationToken token)
    {
        return await _mapLayerRepository.Get(paginator, token);
    }

    public async Task<MapLayer?> Get(Guid id)
    {
        var mapLayer = await _mapLayerRepository.Get(id);
        if (mapLayer is null)
        {
            throw new NotFoundException("Map layer not found");
        }
        
        return mapLayer;
    }

    public async Task<MapLayer> Create(NewMapLayerDto dto)
    {
        var mapLayer = MapLayerMapper.MapToEntity(dto);

        foreach (var featureId in dto.MapFeatureIds)
        {
            mapLayer.MapFeatureLayers.Add(new MapFeatureLayer
            {
                MapFeatureId = featureId,
                MapLayerId = mapLayer.Id
            });
        }

        _mapLayerRepository.Create(mapLayer);
        await _context.SaveChangesAsync();
        
        return mapLayer;
    }

    public async Task<MapLayer> Update(Guid id, UpdateMapLayerDto dto)
    {
        var mapLayer = await _mapLayerRepository.Get(id);
        if (mapLayer is null)
        {
            throw new NotFoundException("Map layer not found");
        }

        MapLayerMapper.UpdateFromDto(dto, mapLayer);

        // Update feature associations
        // Remove existing associations
        mapLayer.MapFeatureLayers.Clear();

        // Add new associations
        foreach (var featureId in dto.MapFeatureIds)
        {
            mapLayer.MapFeatureLayers.Add(new MapFeatureLayer
            {
                MapFeatureId = featureId,
                MapLayerId = mapLayer.Id
            });
        }

        _mapLayerRepository.Update(mapLayer);
        await _context.SaveChangesAsync();

        return mapLayer;
    }
    
    public async Task Delete(Guid id)
    {
        var mapLayer = await _mapLayerRepository.Get(id);
        if (mapLayer is null)
        {
            throw new NotFoundException("Map layer not found");
        }
    
        _mapLayerRepository.Delete(mapLayer);
        await _context.SaveChangesAsync();
    }
}
