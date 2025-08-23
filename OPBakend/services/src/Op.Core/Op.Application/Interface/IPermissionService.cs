namespace Op.Application.Interface;

public interface IPermissionService
{
    Task<IReadOnlyCollection<string>> GetForUserAsync(string userId, string tenantId, CancellationToken ct = default);
    Task<bool> HasAsync(string userId, string tenantId, string permission, CancellationToken ct = default);
}   