using LayerHub.Shared.Dto.Tenant;

namespace LayerHub.Shared.Dto.User;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public TenantDto? Owner { get; set; }
}
