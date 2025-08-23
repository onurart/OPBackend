using Op.Persistance.Tenant;

namespace Hotel.Api.Workers;

public sealed class CompanyProvisioningWorker(
    ICompanyProvisioningQueue queue,
    IServiceScopeFactory scopeFactory,
    ILogger<CompanyProvisioningWorker> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var company in queue.DequeueAllAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var manager = scope.ServiceProvider.GetRequiredService<TenantDbManager>();
                await manager.EnsureCreatedAndMigratedAsync(company, stoppingToken);

                log.LogInformation("Provisioned tenant DB '{Db}' for Company '{Name}' ({Id})",
                    company.Database, company.Name, company.Id);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Provisioning failed for Company {Id}", company.Id);
            }
        }
    }
}