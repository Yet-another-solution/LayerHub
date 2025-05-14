using AutoMapper;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.MapFeature;
using LayerHub.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LayerHub.Api.Apis;

public static class MapFeatureApi
{
    public static RouteGroupBuilder AddMapFeatureApi(this IEndpointRouteBuilder app)
    {
        var api = app
            .MapGroup("Feature")
            .RequireAuthorization();

        api.MapGet("/", GetFeatures)
            .WithName("Get Features");

        api.MapGet("/{id:guid}", GetFeature)
            .WithName("Get Feature");

        api.MapPost("/", CreateFeature)
            .WithName("Create Feature");

        api.MapPut("/{id:guid}", UpdateFeature)
            .WithName("Update Feature");

        api.MapDelete("/{id:guid}", DeleteFeature)
            .WithName("Delete Feature");

        return api;
    }

    public static async Task<Results<Ok<PaginatedListDto<MapFeatureDto>>, ProblemHttpResult>> GetFeatures(
        IMapFeatureService featureService, 
        IMapper mapper, 
        [AsParameters] BasePaginator paginator, 
        CancellationToken cancellationToken)
    {
        var features = await featureService.Get(paginator, cancellationToken);
        var dto = mapper.Map<PaginatedList<MapFeatureDto>>(features);

        return TypedResults.Ok(new PaginatedListDto<MapFeatureDto>(dto));
    }

    public static async Task<Results<Ok<MapFeatureDto>, NotFound, ProblemHttpResult>> GetFeature(
        IMapFeatureService featureService, 
        IMapper mapper, 
        [FromRoute] Guid id)
    {
        var feature = await featureService.Get(id);

        if (feature is null)
        {
            throw new NotFoundException();
        }

        return TypedResults.Ok(mapper.Map<MapFeatureDto>(feature));
    }

    public static async Task<Results<Created<MapFeatureDto>, ProblemHttpResult>> CreateFeature(
        IMapFeatureService featureService, 
        IMapper mapper,
        [FromBody] NewMapFeatureDto dto)
    {
        var feature = await featureService.Create(dto);

        return TypedResults.Created("", mapper.Map<MapFeatureDto>(feature));
    }

    public static async Task<Results<Ok<MapFeatureDto>, NotFound, ProblemHttpResult>> UpdateFeature(
        IMapFeatureService featureService, 
        IMapper mapper, 
        [FromRoute] Guid id,
        [FromBody] UpdateMapFeatureDto dto)
    {
        var feature = await featureService.Update(id, dto);

        return TypedResults.Ok(mapper.Map<MapFeatureDto>(feature));
    }

    public static async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteFeature(
        IMapFeatureService featureService,
        [FromRoute] Guid id)
    {
        await featureService.Delete(id);

        return TypedResults.NoContent();
    }
}
