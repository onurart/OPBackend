using Microsoft.AspNetCore.Authorization;
using OpShared.Security;

namespace Op.Presentatiton.Auth;


public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // 1) Kullanıcı login mi?
        if (context.User?.Identity?.IsAuthenticated != true)
            return Task.CompletedTask;

        // 2) Basit ve hızlı yol: JWT içindeki "perm" claim'leri
        //    Örn: perm=booking.read
        bool has = context.User.HasClaim(ClaimTypesEx.Permission, requirement.Permission);

        if (has)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}