using Blazored.Toast;
using LayerHub.Web.Application.Extension;
using LayerHub.Web.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add serilog
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Services
builder.Services.AddBlazoredToast();
builder.Services.AddFluentUIComponents();
builder.Services.AddServicesAndRepositories();

// Set HttpClient
var globalConfig = builder.Configuration.GetSection("Global");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(globalConfig.GetValue<string>("ApiUrl") ?? throw new NullReferenceException("ApiUrl cannot be null")) });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();