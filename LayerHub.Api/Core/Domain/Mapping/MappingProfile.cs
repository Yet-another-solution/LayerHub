using AutoMapper;
using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Dto.Tenant;
using LayerHub.Shared.Dto.User;
using LayerHub.Shared.Models;
using LayerHub.Shared.Models.Identity;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Domain.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<PaginatedList<ApplicationUser>, PaginatedList<UserDto>>()
            .ConvertUsing<PagedListConverter<ApplicationUser, UserDto>>();

        // Tenant
        CreateMap<Tenant, TenantDto>();
        CreateMap<NewTenantDto, Tenant>();
        CreateMap<UpdateTenantDto, Tenant>();
        CreateMap<PaginatedList<Tenant>, PaginatedList<TenantDto>>()
            .ConvertUsing<PagedListConverter<Tenant, TenantDto>>();

        // MapFeature
        CreateMap<MapFeature, MapFeatureDto>();
        CreateMap<NewMapFeatureDto, MapFeature>();
        CreateMap<UpdateMapFeatureDto, MapFeature>();
        CreateMap<PaginatedList<MapFeature>, PaginatedList<MapFeatureDto>>()
            .ConvertUsing<PagedListConverter<MapFeature, MapFeatureDto>>();

        // MapLayer
        CreateMap<MapLayer, MapLayerDto>()
            .ForMember(dest => dest.MapFeatures, opt => 
                opt.MapFrom(src => src.MapFeatureLayers.Select(mfl => mfl.MapFeature).ToList()));
        CreateMap<NewMapLayerDto, MapLayer>();
        CreateMap<UpdateMapLayerDto, MapLayer>();
        CreateMap<PaginatedList<MapLayer>, PaginatedList<MapLayerDto>>()
            .ConvertUsing<PagedListConverter<MapLayer, MapLayerDto>>();
    }
}
