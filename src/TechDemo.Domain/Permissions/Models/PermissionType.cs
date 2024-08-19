using TechDemo.Domain.Shared.Models;
using TechDemo.Domain.Shared.Result;

namespace TechDemo.Domain.Permissions.Models;

public class PermissionType : Enumeration
{
    public PermissionType(int id, string description) : base(id, description)
    {
    }

    public static PermissionType Owner { get; } = new(1, nameof(Owner).ToLowerInvariant());

    public static PermissionType Admin { get; } = new(2, nameof(Admin).ToLowerInvariant());

    public static PermissionType Editor { get; } = new(3, nameof(Editor).ToLowerInvariant());

    public static PermissionType Publisher { get; } = new(4, nameof(Publisher).ToLowerInvariant());

    public static PermissionType Moderator { get; } = new(5, nameof(Moderator).ToLowerInvariant());

    public static IEnumerable<PermissionType> PermissionTypes = [
        Owner,
        Admin,
        Editor,
        Publisher,
        Moderator
    ];

    public static Result<PermissionType> FromDescription(string description)
    {
        var matchedPermissionType = PermissionTypes.FirstOrDefault(type =>
            string.Equals(type.Description, description, StringComparison.CurrentCultureIgnoreCase));

        if (matchedPermissionType is null)
        {
            return Result.Failure<PermissionType>(Error.InvalidPermissionDescription);
        }

        return matchedPermissionType;
    }
}