namespace Op.Persistance.Identity;

public class RolePermission
{
    public string RoleId { get; set; } = default!;
    public int PermissionId { get; set; }

    public ApplicationRole Role { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}
