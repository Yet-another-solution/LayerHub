namespace LayerHub.Api.Core.Domain.Context;

public class CurrentContextMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, CurrentContext currentContext)
    {
        currentContext.Build(httpContext);
        await _next.Invoke(httpContext);
    }
}
