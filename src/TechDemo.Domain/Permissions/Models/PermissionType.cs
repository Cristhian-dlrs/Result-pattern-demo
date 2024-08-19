using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models;

public class PermissionType : Entity
{
    public string Description { get; }

    private PermissionType(string description)
    {
        Description = description;
    }

    public static PermissionType CreatePermissionType(string description)
        => new PermissionType(description);
}