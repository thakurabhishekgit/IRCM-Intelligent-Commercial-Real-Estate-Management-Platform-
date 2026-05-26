namespace IRCM.DTOs.Property;

public class PropertyResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public decimal Price { get; set; }

    public string PropertyType { get; set; }

    public int TotalUnits { get; set; }

    public int OccupiedUnits { get; set; }

    public decimal MonthlyMaintenanceCost { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public double ROI { get; set; }

    public string Amenities { get; set; }

    public string ThumbnailUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public AgentDto Agent { get; set; }
}

public class AgentDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }
}