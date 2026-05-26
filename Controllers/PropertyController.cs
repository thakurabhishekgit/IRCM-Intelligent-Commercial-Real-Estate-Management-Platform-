using System.Security.Claims;
using IRCM.DTOs.Property;
using IRCM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRCM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(
        IPropertyService propertyService
    )
    {
        _propertyService = propertyService;
    }

    // CREATE PROPERTY

    [Authorize(Roles = "Agent,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProperty(
        CreatePropertyDto dto
    )
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var property =
            await _propertyService.CreatePropertyAsync(
                dto,
                Guid.Parse(userId!)
            );

        return StatusCode(201, new
        {
            success = true,
            message = "Property created successfully",
            data = property
        });
    }

    // GET ALL PROPERTIES

    [HttpGet]
    public async Task<IActionResult> GetAllProperties()
    {
        var properties =
            await _propertyService.GetAllPropertiesAsync();

        return Ok(new
        {
            success = true,
            count = properties.Count,
            data = properties
        });
    }

    // GET PROPERTY BY ID

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPropertyById(
        Guid id
    )
    {
        var property =
            await _propertyService.GetPropertyByIdAsync(id);

        if (property == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Property not found"
            });
        }

        return Ok(new
        {
            success = true,
            data = property
        });
    }

    // UPDATE PROPERTY

    [Authorize(Roles = "Agent,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(
        Guid id,
        UpdatePropertyDto dto
    )
    {
        var property =
            await _propertyService.UpdatePropertyAsync(
                id,
                dto
            );

        if (property == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Property not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Property updated successfully",
            data = property
        });
    }

    // DELETE PROPERTY

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(
        Guid id
    )
    {
        var deleted =
            await _propertyService.DeletePropertyAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = "Property not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Property deleted successfully"
        });
    }
}