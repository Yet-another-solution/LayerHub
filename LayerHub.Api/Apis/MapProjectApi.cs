using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Domain.Mapping;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.MapProject;
using LayerHub.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LayerHub.Api.Apis;

public static class MapProjectApi
{
    public static RouteGroupBuilder AddMapProjectApi(this IEndpointRouteBuilder app)
    {
        var api = app
            .MapGroup("Project")
            .RequireAuthorization();

        api.MapGet("/", GetProjects)
            .WithName("Get Projects");

        api.MapGet("/{id:guid}", GetProject)
            .WithName("Get Project");

        api.MapPost("/", CreateProject)
            .WithName("Create Project");

        api.MapPut("/{id:guid}", UpdateProject)
            .WithName("Update Project");

        api.MapDelete("/{id:guid}", DeleteProject)
            .WithName("Delete Project");

        // Add public endpoint group that doesn't require authorization
        var publicApi = app.MapGroup("Public/Project");
            
        publicApi.MapGet("/{id:guid}", GetPublishedProject)
            .WithName("Get Published Project");
            
        publicApi.MapGet("/{id:guid}/export", ExportMapAsGeoJson)
            .WithName("Export Map As GeoJson")
            .Produces<object>(200, "application/json");
    
        return api;
    }
    
    public static async Task<Results<Ok<MapProjectDto>, NotFound, ProblemHttpResult>> GetPublishedProject(
        IMapProjectService projectService, 
        [FromRoute] Guid id)
    {
        var project = await projectService.GetPublished(id);
    
        if (project is null)
        {
            throw new NotFoundException();
        }
    
        return TypedResults.Ok(MapProjectMapper.MapToDtoWithLayers(project));
    }
    
    // Endpoint to export map data as GeoJSON
    public static async Task<IResult> ExportMapAsGeoJson(
        IMapProjectService projectService, 
        [FromRoute] Guid id)
    {
        var project = await projectService.GetPublished(id);
    
        if (project is null)
        {
            throw new NotFoundException();
        }
        
        var projectDto = MapProjectMapper.MapToDtoWithLayers(project);
        
        // Convert to GeoJSON FeatureCollection
        var features = new List<object>();
        
        foreach (var layer in projectDto.Layers)
        {
            foreach (var feature in layer.MapFeatures)
            {
                var featureObject = new
                {
                    type = "Feature",
                    geometry = System.Text.Json.JsonSerializer.Deserialize<object>(feature.GeometryJson),
                    properties = new 
                    {
                        name = feature.Name,
                        displayName = feature.DisplayName,
                        description = feature.Description,
                        layerName = layer.Name,
                        projectName = projectDto.Name
                    }
                };
                
                features.Add(featureObject);
            }
        }
        
        var geoJson = new
        {
            type = "FeatureCollection",
            features,
            properties = new
            {
                name = projectDto.Name,
                displayName = projectDto.DisplayName,
                exportedAt = DateTimeOffset.UtcNow
            }
        };
        
        return Results.Ok(geoJson);
    }

    public static async Task<Results<Ok<PaginatedListDto<MapProjectDto>>, ProblemHttpResult>> GetProjects(
        IMapProjectService projectService, 
        [AsParameters] BasePaginator paginator, 
        CancellationToken cancellationToken)
    {
        var projects = await projectService.Get(paginator, cancellationToken);
        var dto = MapProjectMapper.MapToPaginatedDtoList(projects);

        return TypedResults.Ok(new PaginatedListDto<MapProjectDto>(dto));
    }

    public static async Task<Results<Ok<MapProjectDto>, NotFound, ProblemHttpResult>> GetProject(
        IMapProjectService projectService, 
        [FromRoute] Guid id)
    {
        var project = await projectService.Get(id);

        if (project is null)
        {
            throw new NotFoundException();
        }

        return TypedResults.Ok(MapProjectMapper.MapToDtoWithLayers(project));
    }

    public static async Task<Results<Created<MapProjectDto>, ProblemHttpResult>> CreateProject(
        IMapProjectService projectService, 
        [FromBody] NewMapProjectDto dto)
    {
        var project = await projectService.Create(dto);

        return TypedResults.Created("", MapProjectMapper.MapToDto(project));
    }

    public static async Task<Results<Ok<MapProjectDto>, NotFound, ProblemHttpResult>> UpdateProject(
        IMapProjectService projectService, 
        [FromRoute] Guid id,
        [FromBody] UpdateMapProjectDto dto)
    {
        var project = await projectService.Update(id, dto);

        return TypedResults.Ok(MapProjectMapper.MapToDto(project));
    }

    public static async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteProject(
        IMapProjectService projectService,
        [FromRoute] Guid id)
    {
        await projectService.Delete(id);

        return TypedResults.NoContent();
    }
}
