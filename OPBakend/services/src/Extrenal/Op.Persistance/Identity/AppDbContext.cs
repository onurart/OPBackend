using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Op.Persistance.Tenant;

namespace Op.Persistance.Identity;

public class AppDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly ICompanyProvisioningQueue _provisionQueue;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        ICompanyProvisioningQueue provisionQueue) : base(options)
    {
        _provisionQueue = provisionQueue;
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<ApplicationUser>().ToTable("AspNetUsers", "auth");
        b.Entity<ApplicationRole>().ToTable("AspNetRoles", "auth");
        b.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", "auth");
        b.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", "auth");
        b.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", "auth");
        b.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", "auth");
        b.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", "auth");

        b.Entity<Permission>().ToTable("Permissions", "auth")
            .HasIndex(x => x.Name).IsUnique();
        b.Entity<RolePermission>().ToTable("RolePermissions", "auth");
        b.Entity<Company>(e =>
        {
            e.ToTable("Companies", "public");
            e.HasIndex(x => x.Name);
            e.Property(x => x.Password).HasMaxLength(256);
        });
        // Permission & RolePermission
        b.Entity<Permission>(e =>
        {
            e.ToTable("Permissions", "auth");
            e.HasIndex(x => x.Name).IsUnique();
        });
        b.Entity<RolePermission>(e =>
        {
            e.ToTable("RolePermissions", "auth");
            e.HasKey(x => new { x.RoleId, x.PermissionId }); // <-- BURASI ZORUNLU
            e.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Cascade);
        });
    }


    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var newlyAdded = ChangeTracker
            .Entries<Company>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();
        var result = await base.SaveChangesAsync(ct);
        foreach (var c in newlyAdded)
            await _provisionQueue.EnqueueAsync(c, ct);

        return result;
    }
}