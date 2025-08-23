namespace OpShared.Security;


public static class ClaimTypesEx
{
    // Standartların yanında kendi claim type'larını sabitle
    public const string UserId     = "uid";
    public const string TenantId   = "tenant_id";
    public const string Permission = "perm";        // örn: "booking.manage"
    public const string Role       = "role";        // istersen
}