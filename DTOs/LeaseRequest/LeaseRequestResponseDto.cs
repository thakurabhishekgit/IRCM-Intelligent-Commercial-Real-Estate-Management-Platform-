namespace IRCM.DTOs.LeaseRequest;

public class LeaseRequestResponseDto
{
    public Guid Id { get; set; }

    public string Message { get; set; }

    public string Status { get; set; }

    public DateTime RequestedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public PropertyDto Property { get; set; }

    public TenantDto Tenant { get; set; }

    public AgentDto Agent { get; set; }
}

public class PropertyDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Location { get; set; }

    public decimal Price { get; set; }

    public string ThumbnailUrl { get; set; }
}

public class TenantDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }
}

public class AgentDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }
}