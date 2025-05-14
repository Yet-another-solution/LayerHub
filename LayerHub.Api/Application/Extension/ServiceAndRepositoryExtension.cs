using LayerHub.Api.Core.Domain.Context;
using LayerHub.Api.Core.Domain.Interfaces;
using LayerHub.Api.Core.Services;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Api.Infrasctructure.Initializer;
using LayerHub.Api.Infrasctructure.Repositories;

namespace LayerHub.Api.Application.Extension;

public static class ServicesAndRepositoryExtension
{
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
    {
        #region Repository

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IMapFeatureRepository, MapFeatureRepository>();

        #endregion
        #region Service

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IMapFeatureService, MapFeatureService>();

        #endregion

        services.AddScoped<CurrentContext>();
        services.AddSingleton(TimeProvider.System);
        
        // Register DB initializer
        services.AddScoped<DbInitializer>();


        return services;
    }
}
