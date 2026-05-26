using IRCM.Enums;

namespace IRCM.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string PhoneNumber { get; set; }

    public UserRole Role { get; set; } = UserRole.Tenant;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties

    // public ICollection<Property> Properties { get; set; }

    // public ICollection<Lease> Leases { get; set; }

    // public ICollection<Document> UploadedDocuments { get; set; }

    // public ICollection<AIInsight> GeneratedInsights { get; set; }
}