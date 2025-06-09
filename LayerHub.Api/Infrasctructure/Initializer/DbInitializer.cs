using System.Globalization;
using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using LayerHub.Api.Core.Domain.Mapping;
using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Models;
using LayerHub.Shared.Models.Identity;
using LayerHub.Shared.ReadDocuments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LayerHub.Api.Infrasctructure.Initializer;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DbInitializer> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;
    private readonly TimeProvider _timeProvider;
    private readonly MongoDbContext _mongoContext;

    public DbInitializer(
        ApplicationDbContext context,
        ILogger<DbInitializer> logger,
        UserManager<ApplicationUser> userManager, 
        IWebHostEnvironment environment, 
        TimeProvider timeProvider, 
        MongoDbContext mongoContext)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
        _environment = environment;
        _timeProvider = timeProvider;
        _mongoContext = mongoContext;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Create database and apply migrations
            await _context.Database.MigrateAsync();

            // Seed data
            await SeedDataAsync();

            _logger.LogInformation("Database initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    private async Task SeedDataAsync()
    {
        await SeedTenantsAsync();
        await SeedUsersAsync();
        await SeedMapFeaturesAsync("Polygon.csv");

        await _context.SaveChangesAsync();
    }

    #region Tenants

    private async Task SeedTenantsAsync()
    {
        if (_context.Tenants.All(t => t.Id != Guid.Parse("3e76f4ef-a76c-4442-a931-573a00475e3d")))
        {
            var defaultTenant = new Tenant
            {
                Id = Guid.Parse("3e76f4ef-a76c-4442-a931-573a00475e3d"),
                Name = "Default Tenant"
            };

            await _context.Tenants.AddAsync(defaultTenant);
        }
    }

    #endregion

    #region Users

    private async Task SeedUsersAsync()
    {
        if (_context.Users.All(u => u.UserName != "mm@mm"))
        {
            var user = new ApplicationUser
            {
                UserName = "mm@mm",
                Email = "mm@mm",
                OwnerId = Guid.Parse("3e76f4ef-a76c-4442-a931-573a00475e3d"),
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user, "P@ssw0rd.+");
        }
    }

    #endregion


    #region MapFeatures

    private async Task SeedMapFeaturesAsync(string csvFilePath)
    {
        // Skip if features already exist
        if (await _context.MapFeatures.IgnoreQueryFilters().AnyAsync())
        {
            _logger.LogInformation("MapFeatures already exist in the database. Skipping import.");
            return;
        }

        string fullPath = csvFilePath;
        if (!Path.IsPathRooted(csvFilePath))
        {
            // If a relative path is provided, assume it's relative to wwwroot
            fullPath = Path.Combine(_environment.WebRootPath, csvFilePath);
        }

        if (!File.Exists(fullPath))
        {
            _logger.LogWarning("CSV file not found at {FilePath}", fullPath);
            return;
        }

        var features = new List<MapFeature>();

        try
        {
            // Configure CsvHelper
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using (var reader = new StreamReader(fullPath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<PolygonCsvMap>();
                var records = csv.GetRecords<PolygonCsvRecord>();
                foreach (var record in records)
                {
                    try
                    {
                        var feature = new MapFeature
                        {
                            Name = record.PolygonName,
                            Description = string.IsNullOrWhiteSpace(record.Description) ? null : record.Description,
                            GeometryJson = JsonDocument.Parse(record.JSON),
                            Size = record.Size,
                            OwnerId = Guid.Parse("3e76f4ef-a76c-4442-a931-573a00475e3d"),
                            CreatedAt = _timeProvider.GetUtcNow(),
                            UpdatedAt = _timeProvider.GetUtcNow()
                        };

                        features.Add(feature);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing polygon record: {Name}", record.PolygonName);
                    }
                }
            }

            if (features.Count > 0)
            {
                await _context.MapFeatures.AddRangeAsync(features);
                _logger.LogInformation("Added {Count} map features from CSV to be saved", features.Count);
                
                // Group features by the first word
                var groupedFeatures = features.GroupBy(f => f.Name.Split(" ")[0]);
                foreach (var group in groupedFeatures)
                {
                    var layer = new MapLayerDocument()
                    {
                        Id = Guid.CreateVersion7(),
                        Name = group.Key,
                        Description = group.Key,
                        OwnerId = Guid.Parse("3e76f4ef-a76c-4442-a931-573a00475e3d")
                    };

                    layer.MapFeatures = group.Select(f => MapFeatureMapper.MapToDocument(f)).ToList();

                    await _mongoContext.MapLayers.InsertOneAsync(layer);
                }
            }
            else
            {
                _logger.LogWarning("No valid map features found in the CSV file");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading CSV file: {FilePath}", fullPath);
        }
    }

    #endregion
}

// CSV Record class for mapping CSV to objects
public class PolygonCsvRecord
{
    [Name("polygonName")] public string PolygonName { get; set; } = "";

    [Name("polygonPoints")] public string PolygonPoints { get; set; } = "";

    [Name("description")] public string Description { get; set; } = "";

    [Name("size")] public int? Size { get; set; }

    [Name("JSON")] public string JSON { get; set; } = "";
}

// Custom mapping class to handle any special CSV mapping requirements
public class PolygonCsvMap : ClassMap<PolygonCsvRecord>
{
    public PolygonCsvMap()
    {
        Map(m => m.PolygonName).Name("polygonName");
        Map(m => m.PolygonPoints).Name("polygonPoints");
        Map(m => m.Description).Name("description");
        Map(m => m.Size).Name("size").Optional();
        Map(m => m.JSON).Name("JSON");
    }
}
