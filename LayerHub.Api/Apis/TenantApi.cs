using AutoMapper;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.Tenant;
using LayerHub.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LayerHub.Api.Apis;

public static class TenantApi
{
    public static RouteGroupBuilder AddTenantApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("Blog");

        api.MapGet("/", GetTenants)
            .WithName("Get Tenants");

        api.MapGet("/{id:guid}", GetTenant)
            .WithName("Get Tenant");

        api.MapPost("/", CreateTenant)
            .WithName("Create Tenant");

        api.MapPut("/{id:guid}", UpdateTenant)
            .WithName("Update Tenant");

        api.MapDelete("/{id:guid}", DeleteTenant)
            .WithName("Delete Tenant");

        return api;
    }

    public static async Task<Results<Ok<PaginatedListDto<TenantDto>>, ProblemHttpResult>> GetTenants(ITenantService tenantService, IMapper mapper, [FromQuery] BasePaginator paginator, CancellationToken cancellationToken)
    {
        var tenants = await tenantService.Get(paginator, cancellationToken);
        var dto = mapper.Map<PaginatedList<TenantDto>>(tenants); 

        return TypedResults.Ok(new PaginatedListDto<TenantDto>(dto));
    }

    public static async Task<Results<Ok<TenantDto>, NotFound, ProblemHttpResult>> GetTenant(ITenantService tenantService, IMapper mapper, [FromRoute] Guid id)
    {
        var tenant = await tenantService.Get(id);

        if (tenant is null)
        {
            throw new NotFoundException();
        }

        return TypedResults.Ok(mapper.Map<TenantDto>(tenant));
    }

    public static async Task<Results<Created<TenantDto>, ProblemHttpResult>> CreateTenant(ITenantService tenantService, IMapper mapper,
        [FromBody] NewTenantDto dto)
    {
        var tenant = await tenantService.Create(dto);

        return TypedResults.Created("", mapper.Map<TenantDto>(tenant));
    }

    public static async Task<Results<Ok<TenantDto>, NotFound, ProblemHttpResult>> UpdateTenant(ITenantService tenantService, IMapper mapper, [FromRoute] Guid id,
        [FromBody] UpdateTenantDto dto)
    {
        var tenant = await tenantService.Update(id, dto);

        return TypedResults.Ok(mapper.Map<TenantDto>(tenant));
    }

    public static async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteTenant(ITenantService tenantService,
        [FromRoute] Guid id)
    {
        await tenantService.Delete(id);

        return TypedResults.NoContent();
    }
}