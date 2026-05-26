using IRCM.Enums;

namespace IRCM.Models;

public class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Location { get; set; }

    public decimal Price { get; set; }

    public PropertyType PropertyType { get; set; } = PropertyType.Office;

    public int TotalUnits { get; set; }

    public int OccupiedUnits { get; set; }

    public decimal MonthlyMaintenanceCost { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public double ROI { get; set; }

    public required string Amenities { get; set; }

    public required string ThumbnailUrl { get; set; }

    public Guid AgentId { get; set; }

    public User Agent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}