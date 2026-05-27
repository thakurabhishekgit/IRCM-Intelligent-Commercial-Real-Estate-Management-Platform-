using IRCM.Enums;

namespace IRCM.Models;

public class Lease
{
    public Guid Id { get; set; }
        = Guid.NewGuid();

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
    // LEASE DETAILS
    // =========================

    public decimal MonthlyRent { get; set; }

    public decimal SecurityDeposit { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public LeaseStatus Status { get; set; }
        = LeaseStatus.Pending;

    // =========================
    // OPTIONAL
    // =========================

    public Guid? LeaseRequestId { get; set; }

    public LeaseRequest? LeaseRequest { get; set; }

    // =========================
    // TIMESTAMPS
    // =========================

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}