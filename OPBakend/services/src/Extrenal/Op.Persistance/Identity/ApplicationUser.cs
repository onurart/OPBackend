using Microsoft.AspNetCore.Identity;

namespace Op.Persistance.Identity;

public class ApplicationUser : IdentityUser
{
    public string? TenantId { get; set; }
    public bool IsActive { get; set; } = true;
}