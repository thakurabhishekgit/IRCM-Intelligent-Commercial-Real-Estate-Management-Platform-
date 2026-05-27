using IRCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IRCM.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }

    public DbSet<LeaseRequest> LeaseRequests { get; set; }

    protected override void OnModelCreating(
    ModelBuilder modelBuilder
)
{
    base.OnModelCreating(modelBuilder);

    // =========================
    // USER
    // =========================

    modelBuilder.Entity<User>()
        .HasIndex(x => x.Email)
        .IsUnique();

    modelBuilder.Entity<User>()
        .Property(x => x.Role)
        .HasConversion<string>();

    // =========================
    // PROPERTY
    // =========================

    modelBuilder.Entity<Property>()
        .Property(x => x.PropertyType)
        .HasConversion<string>();

    modelBuilder.Entity<Property>()
        .HasOne(x => x.Agent)
        .WithMany()
        .HasForeignKey(x => x.AgentId)
        .OnDelete(DeleteBehavior.Restrict);

    // =========================
    // DECIMAL PRECISION
    // =========================

    modelBuilder.Entity<Property>()
        .Property(x => x.Price)
        .HasPrecision(18, 2);

    modelBuilder.Entity<Property>()
        .Property(x => x.MonthlyRevenue)
        .HasPrecision(18, 2);

    modelBuilder.Entity<Property>()
        .Property(x => x.MonthlyMaintenanceCost)
        .HasPrecision(18, 2);

    // =========================
    // LEASE REQUEST
    // =========================

    modelBuilder.Entity<LeaseRequest>()
        .HasOne(x => x.Property)
        .WithMany()
        .HasForeignKey(x => x.PropertyId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<LeaseRequest>()
        .HasOne(x => x.Tenant)
        .WithMany()
        .HasForeignKey(x => x.TenantId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<LeaseRequest>()
        .HasOne(x => x.Agent)
        .WithMany()
        .HasForeignKey(x => x.AgentId)
        .OnDelete(DeleteBehavior.Restrict);
}
}