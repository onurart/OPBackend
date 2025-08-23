using Microsoft.EntityFrameworkCore;
using Op.Domain.Model;
using Op.Persistance.Identity;

namespace Op.Persistance.Tenant;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyDbContext).Assembly);

    DbSet<Test> Tests { get; set; }
    // Buraya otel domain DbSet'lerini ekle:
    // public DbSet<Product> Products => Set<Product>();
    // public DbSet<Booking> Bookings => Set<Booking>();
}