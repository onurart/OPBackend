using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Op.Persistance.Identity;
using Op.Persistance.Tenant;

namespace Hotel.Api.Controller;
[ApiController, Route("admin/tenants")]

public class TenantAdminController : ControllerBase
{
    private readonly AppDbContext _master;
    private readonly TenantDbManager _manager;

    public TenantAdminController(AppDbContext master, TenantDbManager manager)
    {
        _master = master;
        _manager = manager;
    }
    [HttpPost("{companyId:guid}/migrate")]
    public async Task<IActionResult> Migrate(Guid companyId,
        [FromServices] TenantDbManager manager,
        CancellationToken ct)
    {
        var company = await _master.Companies.FindAsync(new object?[] { companyId }, ct);
        if (company is null) return NotFound();

        await manager.EnsureCreatedAndMigratedAsync(company, ct);
        return Ok();
    }
}