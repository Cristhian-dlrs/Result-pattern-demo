namespace TechDemo.Contracts.Permissions;

public record ApiCommandResponse(
    bool IsSuccess,
    int RelatedObjectId,
    string? ErrorMessage);