using Op.Persistance.Identity;

namespace Op.Persistance.Tenant;

public interface ICompanyProvisioningQueue
{
    Task EnqueueAsync(Company company, CancellationToken ct = default);
    IAsyncEnumerable<Company> DequeueAllAsync(CancellationToken ct = default);
}