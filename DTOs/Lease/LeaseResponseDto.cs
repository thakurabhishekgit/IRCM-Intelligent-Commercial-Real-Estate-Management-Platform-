using IRCM.DTOs.LeaseRequest;

namespace IRCM.DTOs.Lease;

public class LeaseResponseDto
{
    public Guid Id { get; set; }

    public decimal MonthlyRent { get; set; }

    public decimal SecurityDeposit { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public PropertyDto Property { get; set; }

    public TenantDto Tenant { get; set; }

    public AgentDto Agent { get; set; }
}