using IRCM.Data;
using IRCM.DTOs.Property;
using IRCM.Interfaces;
using IRCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IRCM.Services.Implementation;

public class PropertyImplementation : IPropertyService
{
    private readonly ApplicationDbContext _context;

    public PropertyImplementation(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PropertyResponseDto> CreatePropertyAsync(
        CreatePropertyDto dto,
        Guid agentId
    )
    {
        var property = new Property
        {
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            Price = dto.Price,
            PropertyType = dto.PropertyType,
            TotalUnits = dto.TotalUnits,
            OccupiedUnits = dto.OccupiedUnits,
            MonthlyMaintenanceCost =
                dto.MonthlyMaintenanceCost,
            MonthlyRevenue = dto.MonthlyRevenue,
            ROI = dto.ROI,
            Amenities = dto.Amenities,
            ThumbnailUrl = dto.ThumbnailUrl,
            AgentId = agentId
        };

        await _context.Properties.AddAsync(property);

        await _context.SaveChangesAsync();

        var agent = await _context.Users
            .FirstAsync(x => x.Id == agentId);

        return new PropertyResponseDto
        {
            Id = property.Id,
            Title = property.Title,
            Description = property.Description,
            Location = property.Location,
            Price = property.Price,
            PropertyType =
                property.PropertyType.ToString(),
            TotalUnits = property.TotalUnits,
            OccupiedUnits = property.OccupiedUnits,
            MonthlyMaintenanceCost =
                property.MonthlyMaintenanceCost,
            MonthlyRevenue = property.MonthlyRevenue,
            ROI = property.ROI,
            Amenities = property.Amenities,
            ThumbnailUrl = property.ThumbnailUrl,
            CreatedAt = property.CreatedAt,
            UpdatedAt = property.UpdatedAt,

            Agent = new AgentDto
            {
                Id = agent.Id,
                FullName = agent.FullName,
                Email = agent.Email
            }
        };
    }

    public async Task<List<PropertyResponseDto>>
        GetAllPropertiesAsync()
    {
        return await _context.Properties
            .Include(x => x.Agent)
            .Select(x => new PropertyResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                Price = x.Price,
                PropertyType =
                    x.PropertyType.ToString(),
                TotalUnits = x.TotalUnits,
                OccupiedUnits = x.OccupiedUnits,
                MonthlyMaintenanceCost =
                    x.MonthlyMaintenanceCost,
                MonthlyRevenue = x.MonthlyRevenue,
                ROI = x.ROI,
                Amenities = x.Amenities,
                ThumbnailUrl = x.ThumbnailUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,

                Agent = new AgentDto
                {
                    Id = x.Agent.Id,
                    FullName = x.Agent.FullName,
                    Email = x.Agent.Email
                }
            })
            .ToListAsync();
    }

    public async Task<PropertyResponseDto?>
        GetPropertyByIdAsync(Guid id)
    {
        return await _context.Properties
            .Include(x => x.Agent)
            .Where(x => x.Id == id)
            .Select(x => new PropertyResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                Price = x.Price,
                PropertyType =
                    x.PropertyType.ToString(),
                TotalUnits = x.TotalUnits,
                OccupiedUnits = x.OccupiedUnits,
                MonthlyMaintenanceCost =
                    x.MonthlyMaintenanceCost,
                MonthlyRevenue = x.MonthlyRevenue,
                ROI = x.ROI,
                Amenities = x.Amenities,
                ThumbnailUrl = x.ThumbnailUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,

                Agent = new AgentDto
                {
                    Id = x.Agent.Id,
                    FullName = x.Agent.FullName,
                    Email = x.Agent.Email
                }
            })
            .FirstOrDefaultAsync();
    }

   public async Task<PropertyResponseDto?>
    UpdatePropertyAsync(
        Guid propertyId,
        Guid loggedInUserId,
        string role,
        UpdatePropertyDto dto
    )
{
    var property = await _context.Properties
        .Include(x => x.Agent)
        .FirstOrDefaultAsync(x => x.Id == propertyId);

    if (property == null)
    {
        return null;
    }

    // =========================
    // OWNERSHIP CHECK
    // =========================

    if (
        role != "Admin" &&
        property.AgentId != loggedInUserId
    )
    {
        return null;
    }

    // =========================
    // UPDATE
    // =========================

    property.Title = dto.Title;
    property.Description = dto.Description;
    property.Location = dto.Location;
    property.Price = dto.Price;
    property.PropertyType = dto.PropertyType;
    property.TotalUnits = dto.TotalUnits;
    property.OccupiedUnits = dto.OccupiedUnits;

    property.MonthlyMaintenanceCost =
        dto.MonthlyMaintenanceCost;

    property.MonthlyRevenue =
        dto.MonthlyRevenue;

    property.ROI = dto.ROI;

    property.Amenities = dto.Amenities;

    property.ThumbnailUrl = dto.ThumbnailUrl;

    property.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return new PropertyResponseDto
    {
        Id = property.Id,
        Title = property.Title,
        Description = property.Description,
        Location = property.Location,
        Price = property.Price,
        PropertyType =
            property.PropertyType.ToString(),
        TotalUnits = property.TotalUnits,
        OccupiedUnits = property.OccupiedUnits,
        MonthlyMaintenanceCost =
            property.MonthlyMaintenanceCost,
        MonthlyRevenue = property.MonthlyRevenue,
        ROI = property.ROI,
        Amenities = property.Amenities,
        ThumbnailUrl = property.ThumbnailUrl,
        CreatedAt = property.CreatedAt,
        UpdatedAt = property.UpdatedAt,

        Agent = new AgentDto
        {
            Id = property.Agent.Id,
            FullName = property.Agent.FullName,
            Email = property.Agent.Email
        }
    };
}

    public async Task<bool> DeletePropertyAsync(Guid id)
    {
        var property = await _context.Properties
            .FirstOrDefaultAsync(x => x.Id == id);

        if (property == null)
        {
            return false;
        }

        _context.Properties.Remove(property);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<PropertyResponseDto>>
        GetMyPropertiesAsync(Guid agentId)
    {
        return await _context.Properties
            .Include(x => x.Agent)

            .Where(x => x.AgentId == agentId)

            .Select(x => new PropertyResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                Price = x.Price,
                PropertyType =
                    x.PropertyType.ToString(),
                TotalUnits = x.TotalUnits,
                OccupiedUnits = x.OccupiedUnits,
                MonthlyMaintenanceCost =
                    x.MonthlyMaintenanceCost,
                MonthlyRevenue = x.MonthlyRevenue,
                ROI = x.ROI,
                Amenities = x.Amenities,
                ThumbnailUrl = x.ThumbnailUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,

                Agent = new AgentDto
                {
                    Id = x.Agent.Id,
                    FullName = x.Agent.FullName,
                    Email = x.Agent.Email
                }
            })
            .ToListAsync();
    }

    public async Task<PropertyResponseDto?>
        GetMyPropertyByIdAsync(
            Guid propertyId,
            Guid agentId
        )
    {
        return await _context.Properties
            .Include(x => x.Agent)

            .Where(x =>
                x.Id == propertyId &&
                x.AgentId == agentId
            )

            .Select(x => new PropertyResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                Price = x.Price,
                PropertyType =
                    x.PropertyType.ToString(),
                TotalUnits = x.TotalUnits,
                OccupiedUnits = x.OccupiedUnits,
                MonthlyMaintenanceCost =
                    x.MonthlyMaintenanceCost,
                MonthlyRevenue = x.MonthlyRevenue,
                ROI = x.ROI,
                Amenities = x.Amenities,
                ThumbnailUrl = x.ThumbnailUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,

                Agent = new AgentDto
                {
                    Id = x.Agent.Id,
                    FullName = x.Agent.FullName,
                    Email = x.Agent.Email
                }
            })

            .FirstOrDefaultAsync();
    }
}