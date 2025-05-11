using LayerHub.Api.Infrasctructure.Data;
using LayerHub.Shared.Models;
using LayerHub.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LayerHub.Api.Infrasctructure.Initializer;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DbInitializer> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public DbInitializer(
        ApplicationDbContext context,
        ILogger<DbInitializer> logger, 
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
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
}