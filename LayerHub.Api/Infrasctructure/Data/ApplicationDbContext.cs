using LayerHub.Api.Core.Domain.Context;
using LayerHub.Api.Infrasctructure.Data.Conventions;
using LayerHub.Api.Infrasctructure.Util;
using LayerHub.Shared.Models;
using LayerHub.Shared.Models.Identity;
using LayerHub.Shared.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LayerHub.Api.Infrasctructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly Guid? _currentTenantId;
    private readonly bool _isAdmin;
    private readonly TimeProvider _now;

    public virtual DbSet<Tenant> Tenants { get; set; } = default!;
    // Models
    public virtual DbSet<MapFeature> MapFeatures { get; set; } = default!;
    public virtual DbSet<MapFeatureLayer> MapFeatureLayers { get; set; } = default!;
    public virtual DbSet<MapLayer> MapLayers { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, CurrentContext currentContext, TimeProvider now)
        : base(options)
    {
        _currentTenantId = currentContext.TenantId;
        _isAdmin = currentContext.IsAdmin();
        _now = now;

        // Subscribe to change tracker events
        ChangeTracker.Tracked += OnEntityTracked;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    // For testing purposes
    public ApplicationDbContext()
    {
        _now = TimeProvider.System;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        builder.Entity<MapFeatureLayer>()
            .HasKey(mfl => new { mfl.MapFeatureId, mfl.MapLayerId });

        // Query filters
        builder.Model.GetEntityTypes()
            .ToList()
            .ForEach(entityType =>
            {
                if (typeof(IOwned).IsAssignableFrom(entityType.ClrType) &&
                    entityType.ClrType != typeof(ApplicationUser))
                {
                    builder.Entity(entityType.ClrType)
                        .AddQueryFilter<IOwned>(e => e.OwnerId == _currentTenantId || _isAdmin);
                }
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                        .AddQueryFilter<ISoftDeletable>(e => e.DeletedAt == null);
                }
            });

        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Conventions.Add(_ => new GuidV7Convention());
    }

    void OnEntityTracked(object? sender, EntityTrackedEventArgs e)
    {
        if (e.Entry.State is EntityState.Added && e.Entry.Entity is ITrackable trackableEntity)
        {
            trackableEntity.CreatedAt = _now.GetUtcNow();
            trackableEntity.UpdatedAt = _now.GetUtcNow();
        }

        if (e.Entry.State is EntityState.Added && e.Entry.Entity is IOwned ownedEntity)
        {
            if (ownedEntity.OwnerId == default)
            {
                ownedEntity.OwnerId = _currentTenantId ?? throw new InvalidOperationException("TenantId is null");
            }
        }
    }

    void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.NewState is EntityState.Modified && e.Entry.Entity is ITrackable entity)
        {
            entity.UpdatedAt = _now.GetUtcNow();
        }
    }
}
