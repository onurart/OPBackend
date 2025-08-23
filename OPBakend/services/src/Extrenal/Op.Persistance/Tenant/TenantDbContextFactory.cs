using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Op.Persistance.Identity;

namespace Op.Persistance.Tenant;

public sealed class TenantDbContextFactory(AppDbContext master)
    : ITenantDbContextFactory
{
    public async Task<CompanyDbContext> CreateAsync(Guid companyId, CancellationToken ct = default)
    {
        var c = await master.Companies.AsNoTracking()
            .FirstAsync(x => x.Id == companyId, ct);

        var options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseNpgsql($"Host={c.Host};Port={c.Port};Database={c.Database};Username={c.Username};Password={c.Password};" +
                       "SSL Mode=Prefer;Trust Server Certificate=true;Pooling=true;Maximum Pool Size=100;Command Timeout=180;")
            .Options;

        return new CompanyDbContext(options);
    }
}