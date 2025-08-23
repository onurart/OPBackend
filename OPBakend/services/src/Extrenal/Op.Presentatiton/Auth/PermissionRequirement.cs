
using Microsoft.AspNetCore.Authorization;

namespace Op.Presentatiton.Auth;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}