using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Op.Persistance.Tenant;


public class CompanyDbContextFactory : IDesignTimeDbContextFactory<CompanyDbContext>
{
    public CompanyDbContext CreateDbContext(string[] args)
    {
        var b = new DbContextOptionsBuilder<CompanyDbContext>();
        // Design-time için bir TEMEL bağlantı; model aynı olduğu için yeterli
        b.UseNpgsql("Host=localhost;Port=5432;Database=tenant_template;Username=Hotel;Password=Hotel");
        return new CompanyDbContext(b.Options);
    }
}