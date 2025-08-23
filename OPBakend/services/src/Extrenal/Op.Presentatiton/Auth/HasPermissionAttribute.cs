using Microsoft.AspNetCore.Authorization;

namespace Op.Presentatiton.Auth;


// Kullanım: [HasPermission(Permissions.Booking.Read)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "perm:";

    public HasPermissionAttribute(string permission)
        => Policy = PolicyPrefix + permission;
}