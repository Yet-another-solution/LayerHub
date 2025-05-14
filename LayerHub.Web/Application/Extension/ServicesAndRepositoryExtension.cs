using Blazored.LocalStorage;
using LayerHub.Web.Application.Authentication;
using LayerHub.Web.Application.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace LayerHub.Web.Application.Extension;

public static class ServicesAndRepositoryExtension
{
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
    {
        #region Repository



        #endregion
        #region Service

        // Add authentication services
        services.AddAuthenticationCore();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<TenantRouteService>();

        #endregion

        services.AddBlazoredLocalStorage();

        return services;
    }
}
