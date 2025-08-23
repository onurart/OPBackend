using Op.Persistance.Identity;

namespace Op.Persistance.Tenant;

public interface ITenantDbContextFactory
{
    Task<CompanyDbContext> CreateAsync(Guid companyId, CancellationToken ct = default);
}