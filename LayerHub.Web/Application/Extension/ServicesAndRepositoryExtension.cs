using Blazored.LocalStorage;
using LayerHub.Web.Application.Services;

namespace LayerHub.Web.Application.Extension;

public static class ServicesAndRepositoryExtension
{
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
    {
        #region Repository



        #endregion
        #region Service

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<TenantRouteService>();

        #endregion

        services.AddBlazoredLocalStorage();

        return services;
    }
}
