using AutoMapper;
using LayerHub.Api.Apis;
using LayerHub.Api.Core.Domain.Exceptions;
using LayerHub.Api.Core.Domain.Mapping;
using LayerHub.Api.Core.Services.Interfaces;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.Tenant;
using LayerHub.Shared.Models;
using LayerHub.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;


namespace LayerHub.Api.Tests.Controllers;

public class TenantControllerTests
{
    private readonly ITenantService _tenantService;
    private readonly IMapper _mapper;

    public TenantControllerTests()
    {
        _tenantService = Substitute.For<ITenantService>();

        // Setup mapper with actual mapping configuration
        // Replace MappingProfile with your actual mapping profile
        var myProfile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public async Task Test_GetTenants()
    {
        // Arrange
        var paginator = new BasePaginator();
        var cancellationToken = CancellationToken.None;
        var tenants = new PaginatedList<Tenant>(new List<Tenant>()
        {
            new Tenant()
            {
                Name = "Test Tenant"
            }
        }, 1, 0, 0, 20);

        _tenantService.Get(paginator, cancellationToken).Returns(tenants);

        // Act
        var result = await TenantApi.GetTenants(_tenantService, _mapper, paginator, cancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PaginatedListDto<TenantDto>>>(result.Result);
        var returnValue = okResult.Value;
        Assert.NotNull(returnValue);
        Assert.Single(returnValue.Items);
        Assert.Equal("Test Tenant", returnValue.Items[0].Name);
        await _tenantService.Received(1).Get(paginator, cancellationToken);
    }

    [Fact]
    public async Task Test_GetTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant()
        {
            Name = "Test Tenant",
            Id = tenantId
        };

        _tenantService.Get(tenantId).Returns(tenant);

        // Act
        var result = await TenantApi.GetTenant(_tenantService, _mapper, tenantId);

        // Assert
        var okResult = Assert.IsType<Ok<TenantDto>>(result.Result);
        var returnValue = okResult.Value;
        Assert.Equal(tenant.Name, returnValue.Name);
        Assert.Equal(tenant.Id, returnValue.Id);
        await _tenantService.Received(1).Get(tenantId);
    }

    [Fact]
    public async Task Test_GetTenant_NotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        _tenantService.Get(tenantId).Returns((Tenant)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => TenantApi.GetTenant(_tenantService, _mapper, tenantId));
        await _tenantService.Received(1).Get(tenantId);
    }

    [Fact]
    public async Task Test_CreateTenant()
    {
        // Arrange
        var newTenantDto = new NewTenantDto()
        {
            Name = "New Tenant"
        };
        var tenant = new Tenant()
        {
            Name = "New Tenant",
            Id = Guid.NewGuid()
        };

        _tenantService.Create(newTenantDto).Returns(tenant);

        // Act
        var result = await TenantApi.CreateTenant(_tenantService, _mapper, newTenantDto);

        // Assert
        var createdResult = Assert.IsType<Created<TenantDto>>(result.Result);
        var returnValue = createdResult.Value;
        Assert.Equal(tenant.Name, returnValue.Name);
        Assert.Equal(tenant.Id, returnValue.Id);
        await _tenantService.Received(1).Create(newTenantDto);
    }

    [Fact]
    public async Task Test_UpdateTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var updateTenantDto = new UpdateTenantDto()
        {
            Name = "Updated Tenant"
        };
        var tenant = new Tenant()
        {
            Name = "Updated Tenant",
            Id = tenantId
        };

        _tenantService.Update(tenantId, updateTenantDto).Returns(tenant);

        // Act
        var result = await TenantApi.UpdateTenant(_tenantService, _mapper, tenantId, updateTenantDto);

        // Assert
        var okResult = Assert.IsType<Ok<TenantDto>>(result.Result);
        var returnValue = okResult.Value;
        Assert.Equal(tenant.Name, returnValue.Name);
        Assert.Equal(tenant.Id, returnValue.Id);
        await _tenantService.Received(1).Update(tenantId, updateTenantDto);
    }

    [Fact]
    public async Task Test_DeleteTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var result = await TenantApi.DeleteTenant(_tenantService, tenantId);

        // Assert
        Assert.IsType<NoContent>(result.Result);
        await _tenantService.Received(1).Delete(tenantId);
    }
}