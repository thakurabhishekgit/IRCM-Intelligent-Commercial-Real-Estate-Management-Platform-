using IRCM.Enums;
using IRCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IRCM.Data;

public class ApplicationDbContext
    : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext>
            options
    ) : base(options)
    {
    }

    // =========================
    // DBSets
    // =========================

    public DbSet<User> Users { get; set; }

    public DbSet<Property> Properties
    {
        get;
        set;
    }

    public DbSet<LeaseRequest>
        LeaseRequests
    {
        get;
        set;
    }

    public DbSet<Lease> Leases
    {
        get;
        set;
    }

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
            .OnDelete(
                DeleteBehavior.Restrict
            );

        // =========================
        // PROPERTY DECIMALS
        // =========================

        modelBuilder.Entity<Property>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Property>()
            .Property(
                x => x.MonthlyRevenue
            )
            .HasPrecision(18, 2);

        modelBuilder.Entity<Property>()
            .Property(
                x =>
                    x.MonthlyMaintenanceCost
            )
            .HasPrecision(18, 2);

        // =========================
        // LEASE REQUEST
        // =========================

        modelBuilder.Entity<LeaseRequest>()
            .Property(x => x.Status)
            .HasConversion<string>();

        modelBuilder.Entity<LeaseRequest>()
            .HasOne(x => x.Property)
            .WithMany()
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(
                DeleteBehavior.Restrict
            );

        modelBuilder.Entity<LeaseRequest>()
            .HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(
                DeleteBehavior.Restrict
            );

        modelBuilder.Entity<LeaseRequest>()
            .HasOne(x => x.Agent)
            .WithMany()
            .HasForeignKey(x => x.AgentId)
            .OnDelete(
                DeleteBehavior.Restrict
            );

        // =========================
        // LEASE
        // =========================

        modelBuilder.Entity<Lease>()
            .Property(x => x.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Lease>()
            .Property(x => x.MonthlyRent)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Lease>()
            .Property(
                x => x.SecurityDeposit
            )
            .HasPrecision(18, 2);

        modelBuilder.Entity<Lease>()
            .HasOne(x => x.Property)
            .WithMany()
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(
                DeleteBehavior.Restrict
            );

        modelBuilder.Entity<Lease>()
            .HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(
                DeleteBehavior.Restrict
            );

        modelBuilder.Entity<Lease>()
            .HasOne(x => x.Agent)
            .WithMany()
            .HasForeignKey(x => x.AgentId)
            .OnDelete(
                DeleteBehavior.Restrict
            );

        modelBuilder.Entity<Lease>()
            .HasOne(x => x.LeaseRequest)
            .WithMany()
            .HasForeignKey(
                x => x.LeaseRequestId
            )
            .OnDelete(
                DeleteBehavior.Restrict
            );

        modelBuilder.Entity<Lease>()
            .HasIndex(x => x.AgentId);

        modelBuilder.Entity<Lease>()
            .HasIndex(x => x.TenantId);

        modelBuilder.Entity<Lease>()
            .HasIndex(x => x.PropertyId);

        modelBuilder.Entity<LeaseRequest>()
            .HasIndex(x => x.AgentId);

        modelBuilder.Entity<LeaseRequest>()
            .HasIndex(x => x.TenantId);

        modelBuilder.Entity<LeaseRequest>()
            .HasIndex(x => x.PropertyId);
    }
}