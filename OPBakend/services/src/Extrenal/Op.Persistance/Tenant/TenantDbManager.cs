using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Op.Persistance.Identity;

namespace Op.Persistance.Tenant;
public sealed class TenantDbManager(IConfiguration cfg)
{
    public async Task EnsureCreatedAndMigratedAsync(Company c, CancellationToken ct = default)
    {
        // A) Master bağlantısı (HotelMaster) ile sunucuya bağlan
        var masterConn = cfg.GetConnectionString("Master")!; // Host/Port/HotelMaster/Hotel
        await using (var conn = new NpgsqlConnection(masterConn))
        {
            await conn.OpenAsync(ct);

            // DB var mı?
            await using (var check = new NpgsqlCommand(
                "SELECT 1 FROM pg_database WHERE datname = @db", conn))
            {
                check.Parameters.AddWithValue("db", c.Database);
                var exists = await check.ExecuteScalarAsync(ct) is not null;

                if (!exists)
                {
                    // 1) DB'yi oluştur (OWNER belirtmeden)
                    var createSql = $"""CREATE DATABASE "{c.Database}";""";
                    await using (var create = new NpgsqlCommand(createSql, conn))
                        await create.ExecuteNonQueryAsync(ct);

                    // 2) Sahipliğini tenant kullanıcısına ver
                    var alterSql = $"""ALTER DATABASE "{c.Database}" OWNER TO "{c.Username}";""";
                    await using (var alter = new NpgsqlCommand(alterSql, conn))
                        await alter.ExecuteNonQueryAsync(ct);
                }
            }
        }

        // B) Tenant kullanıcısı ile yeni DB’ye bağlan ve migration uygula
        var tenantConn =
            $"Host={c.Host};Port={c.Port};Database={c.Database};Username={c.Username};Password={c.Password};" +
            "SSL Mode=Prefer;Trust Server Certificate=true;Pooling=true;Maximum Pool Size=100;Command Timeout=180;";

        var opts = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseNpgsql(tenantConn, npg => { npg.EnableRetryOnFailure(); npg.CommandTimeout(180); })
            .Options;

        await using var tenantDb = new CompanyDbContext(opts);
        await tenantDb.Database.MigrateAsync(ct);
    }
}