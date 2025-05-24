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
builder.Services.AddHttpClient("API", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var apiUrl = configuration["services:Api:http:0"] ?? "http://Api:5000/";
    client.BaseAddress = new Uri(apiUrl);
});



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