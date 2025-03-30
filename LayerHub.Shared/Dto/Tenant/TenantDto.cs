using System.Collections.ObjectModel;
using LayerHub.Shared.Dto.User;

namespace LayerHub.Shared.Dto.Tenant;

public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public IEnumerable<UserDto> Users { get; set; } = new Collection<UserDto>();
}
