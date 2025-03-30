using System.Collections.ObjectModel;
using LayerHub.Shared.Models.Identity;
using LayerHub.Shared.Models.Interfaces;

namespace LayerHub.Shared.Models;

public class Tenant : ISoftDeletable
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public IEnumerable<ApplicationUser> Users { get; set; } = new Collection<ApplicationUser>();

    // Interfaces
    public DateTimeOffset? DeletedAt { get; set; }
}