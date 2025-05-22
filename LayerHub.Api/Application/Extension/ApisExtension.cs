using LayerHub.Api.Apis;

namespace LayerHub.Api.Application.Extension;

public static class ApisExtension
{
    public static WebApplication AddApis(this WebApplication app)
    {
        app.AddAuthApi();
        app.AddTenantApi();
        app.AddMapFeatureApi();
        app.AddMapLayerApi();

        return app;
    }
}