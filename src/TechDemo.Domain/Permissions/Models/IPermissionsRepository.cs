using TechDemo.Domain.Shared.Result;

namespace TechDemo.Domain.Permissions.Models;

public interface IPermissionsRepository
{
    public Task<Result<Permission>> GetByIdAsync(int id);
    public Task<Result> CreateAsync(Permission permission);
    public Task<Result> UpdateAsync(Permission permission);
}