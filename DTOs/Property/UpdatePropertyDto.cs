using IRCM.Enums;

namespace IRCM.DTOs.Property;

public class UpdatePropertyDto
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Location { get; set; }

    public decimal Price { get; set; }

    public PropertyType PropertyType { get; set; }

    public int TotalUnits { get; set; }

    public int OccupiedUnits { get; set; }

    public decimal MonthlyMaintenanceCost { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public double ROI { get; set; }

    public required string Amenities { get; set; }

    public required string ThumbnailUrl { get; set; }
}