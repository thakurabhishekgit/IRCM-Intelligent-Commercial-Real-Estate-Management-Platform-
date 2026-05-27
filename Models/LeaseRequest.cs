using IRCM.Enums;

namespace IRCM.Models;

public class LeaseRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // =========================
    // PROPERTY
    // =========================

    public Guid PropertyId { get; set; }

    public Property Property { get; set; }

    // =========================
    // TENANT
    // =========================

    public Guid TenantId { get; set; }

    public User Tenant { get; set; }

    // =========================
    // AGENT
    // =========================

    public Guid AgentId { get; set; }

    public User Agent { get; set; }

    // =========================
    // REQUEST DETAILS
    // =========================

    public string Message { get; set; } = string.Empty;

    public LeaseRequestStatus Status { get; set; }
        = LeaseRequestStatus.Pending;

    // =========================
    // TIMESTAMPS
    // =========================

    public DateTime RequestedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }
}