using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Op.Persistance.Identity;
using Microsoft.Extensions.Hosting;
namespace Op.Persistance.Tenant;

public sealed class TenantMigrationWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<TenantMigrationWorker> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var master = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var factory = scope.ServiceProvider.GetRequiredService<ITenantDbContextFactory>();

        var companies = await master.Companies.Where(c => c.IsActive).AsNoTracking().ToListAsync(ct);

        foreach (var c in companies)
        {
            try
            {
                await using var tdb = await factory.CreateAsync(c.Id, ct);
                await tdb.Database.MigrateAsync(ct);
                log.LogInformation("Migrated tenant db for {Company} ({Id})", c.Name, c.Id);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Tenant migration failed for {Id}", c.Id);
            }
        }
    }
}