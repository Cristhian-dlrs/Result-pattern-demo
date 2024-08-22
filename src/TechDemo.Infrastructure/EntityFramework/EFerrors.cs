using TechDemo.Domain.Shared.Results;

namespace TechDemo.Infrastructure.EntityFramework;

public static class EFerrors
{
    public static readonly Error SaveError = new(
        "EFerrors.SaveError",
        "Unable to persist the changes.");
}