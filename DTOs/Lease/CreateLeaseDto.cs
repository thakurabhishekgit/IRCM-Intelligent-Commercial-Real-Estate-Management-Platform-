namespace IRCM.DTOs.Lease;

public class CreateLeaseDto
{
    public Guid PropertyId { get; set; }

    public Guid TenantId { get; set; }

    public Guid LeaseRequestId { get; set; }

    public decimal MonthlyRent { get; set; }

    public decimal SecurityDeposit { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}