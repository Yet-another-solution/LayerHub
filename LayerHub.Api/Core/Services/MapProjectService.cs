using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Core.Domain.Mapping;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Dto.MapProject;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services;

public class MapProjectService : IMapProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapProjectRepository _mapProjectRepository;

    public MapProjectService(ApplicationDbContext context, IMapProjectRepository mapProjectRepository)
    {
        _context = context;
        _mapProjectRepository = mapProjectRepository;
    }

    public async Task<PaginatedList<MapProject>> Get(BasePaginator paginator, CancellationToken token)
    {
        return await _mapProjectRepository.Get(paginator, token);
    }

    public async Task<MapProject?> Get(Guid id)
    {
        var mapProject = await _mapProjectRepository.Get(id);
        if (mapProject is null)
        {
            throw new NotFoundException("Map project not found");
        }
        
        return mapProject;
    }

    public async Task<MapProject?> GetPublished(Guid id)
    {
        var mapProject = await _mapProjectRepository.GetPublished(id);
        if (mapProject is null)
        {
            throw new NotFoundException("Published map project not found");
        }
        
        return mapProject;
    }

    public async Task<MapProject> Create(NewMapProjectDto dto)
    {
        var mapProject = MapProjectMapper.MapToEntity(dto);
        if (string.IsNullOrWhiteSpace(mapProject.Name))
        {
            
        }

        foreach (var layerId in dto.LayerIds)
        {
            mapProject.MapProjectLayers.Add(new MapProjectLayer
            {
                MapLayerId = layerId,
                MapProjectId = mapProject.Id
            });
        }

        _mapProjectRepository.Create(mapProject);
        await _context.SaveChangesAsync();
        
        return mapProject;
    }

    public async Task<MapProject> Update(Guid id, UpdateMapProjectDto dto)
    {
        var mapProject = await _mapProjectRepository.Get(id);
        if (mapProject is null)
        {
            throw new NotFoundException("Map project not found");
        }

        MapProjectMapper.UpdateFromDto(dto, mapProject);

        // Update layer associations
        // Remove existing associations
        mapProject.MapProjectLayers.Clear();

        // Add new associations
        foreach (var layerId in dto.LayerIds)
        {
            mapProject.MapProjectLayers.Add(new MapProjectLayer
            {
                MapLayerId = layerId,
                MapProjectId = mapProject.Id
            });
        }

        _mapProjectRepository.Update(mapProject);
        await _context.SaveChangesAsync();

        return mapProject;
    }
    
    public async Task Delete(Guid id)
    {
        var mapProject = await _mapProjectRepository.Get(id);
        if (mapProject is null)
        {
            throw new NotFoundException("Map project not found");
        }
    
        _mapProjectRepository.Delete(mapProject);
        await _context.SaveChangesAsync();
    }
}
