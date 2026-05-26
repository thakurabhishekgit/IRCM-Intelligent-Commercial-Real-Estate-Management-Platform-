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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(x => x.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(x => x.PropertyType)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .HasOne(x => x.Agent)
            .WithMany()
            .HasForeignKey(x => x.AgentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}