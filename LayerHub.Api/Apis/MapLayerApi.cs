using System.Text.Json;
using AutoMapper;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.MapLayer;
using LayerHub.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LayerHub.Api.Apis;

public static class MapLayerApi
{
    public static RouteGroupBuilder AddMapLayerApi(this IEndpointRouteBuilder app)
    {
        var api = app
            .MapGroup("Layer")
            .RequireAuthorization();

        api.MapGet("/", GetLayers)
            .WithName("Get Layers");

        api.MapGet("/{id:guid}", GetLayer)
            .WithName("Get Layer");

        api.MapPost("/", CreateLayer)
            .WithName("Create Layer");

        api.MapPut("/{id:guid}", UpdateLayer)
            .WithName("Update Layer");

        api.MapDelete("/{id:guid}", DeleteLayer)
            .WithName("Delete Layer");

        return api;
    }

    public static async Task<Results<Ok<PaginatedListDto<MapLayerDto>>, ProblemHttpResult>> GetLayers(
        IMapLayerService layerService, 
        IMapper mapper, 
        [AsParameters] BasePaginator paginator, 
        CancellationToken cancellationToken)
    {
        var layers = await layerService.Get(paginator, cancellationToken);
        var dto = mapper.Map<PaginatedList<MapLayerDto>>(layers);

        return TypedResults.Ok(new PaginatedListDto<MapLayerDto>(dto));
    }

    public static async Task<Results<Ok<MapLayerDto>, NotFound, ProblemHttpResult>> GetLayer(
        IMapLayerService layerService, 
        IMapper mapper, 
        [FromRoute] Guid id)
    {
        var layer = await layerService.Get(id);

        if (layer is null)
        {
            throw new NotFoundException();
        }

        return TypedResults.Ok(mapper.Map<MapLayerDto>(layer));
    }

    public static async Task<Results<Created<MapLayerDto>, ProblemHttpResult>> CreateLayer(
        IMapLayerService layerService, 
        IMapper mapper,
        [FromBody] NewMapLayerDto dto)
    {
        var layer = await layerService.Create(dto);

        return TypedResults.Created("", mapper.Map<MapLayerDto>(layer));
    }

    public static async Task<Results<Ok<MapLayerDto>, NotFound, ProblemHttpResult>> UpdateLayer(
        IMapLayerService layerService, 
        IMapper mapper, 
        [FromRoute] Guid id,
        [FromBody] UpdateMapLayerDto dto)
    {
        var layer = await layerService.Update(id, dto);

        return TypedResults.Ok(mapper.Map<MapLayerDto>(layer));
    }

    public static async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteLayer(
        IMapLayerService layerService,
        [FromRoute] Guid id)
    {
        await layerService.Delete(id);

        return TypedResults.NoContent();
    }
}
