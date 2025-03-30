using LayerHub.Shared.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LayerHub.Shared.Models.Identity;

public class ApplicationUser : IdentityUser<Guid>, ITrackable, IOwned, ISoftDeletable
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Interfaces
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid OwnerId { get; set; }
    public Tenant? Owner { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
