namespace OpShared.Abstractions;

public interface IUserContext
{
    string? UserId { get; }
    string? TenantId { get; }
    IReadOnlyCollection<string> Roles { get; }
    IReadOnlyCollection<string> Permissions { get; }
}