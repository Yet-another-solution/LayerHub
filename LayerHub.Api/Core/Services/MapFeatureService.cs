using AutoMapper;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Services;

public class MapFeatureService : IMapFeatureService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapFeatureRepository _mapFeatureRepository;
    private readonly IMapper _mapper;

    public MapFeatureService(ApplicationDbContext context, IMapFeatureRepository mapFeatureRepository, IMapper mapper)
    {
        _context = context;
        _mapFeatureRepository = mapFeatureRepository;
        _mapper = mapper;
    }

    public async Task<List<MapFeature>> Get(BasePaginator paginator, CancellationToken token)
    {
        return await _mapFeatureRepository.Get(paginator, token);
    }

    public async Task<MapFeature?> Get(Guid id)
    {
        var mapFeature = await _mapFeatureRepository.Get(id);
        if (mapFeature is null)
        {
            throw new NotFoundException("Map feature not found");
        }
        
        return mapFeature;
    }

    public async Task<MapFeature> Create(NewMapFeatureDto dto)
    {
        var mapFeature = _mapper.Map<MapFeature>(dto);
        
        _mapFeatureRepository.Create(mapFeature);
        await _context.SaveChangesAsync();
        
        return mapFeature;
    }

    public async Task<MapFeature> Update(Guid id, UpdateMapFeatureDto dto)
    {
        var mapFeature = await _mapFeatureRepository.Get(id);
        if (mapFeature is null)
        {
            throw new NotFoundException("Map feature not found");
        }

        mapFeature = _mapper.Map(dto, mapFeature);

        _mapFeatureRepository.Update(mapFeature);
        await _context.SaveChangesAsync();

        return mapFeature;
    }
    
    public async Task Delete(Guid id)
    {
        var mapFeature = await _mapFeatureRepository.Get(id);
        if (mapFeature is null)
        {
            throw new NotFoundException("Map feature not found");
        }
    
        _mapFeatureRepository.Delete(mapFeature);
        await _context.SaveChangesAsync();
    }
}