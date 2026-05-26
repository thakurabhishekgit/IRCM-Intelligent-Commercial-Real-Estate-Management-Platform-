using IRCM.DTOs.Property;

namespace IRCM.Interfaces;

public interface IPropertyService
{
    Task<PropertyResponseDto> CreatePropertyAsync(
        CreatePropertyDto dto,
        Guid agentId
    );

    Task<List<PropertyResponseDto>> GetAllPropertiesAsync();

    Task<PropertyResponseDto?> GetPropertyByIdAsync(Guid id);

    Task<PropertyResponseDto?>
    UpdatePropertyAsync(
        Guid propertyId,
        Guid loggedInUserId,
        string role,
        UpdatePropertyDto dto
    );

    Task<bool> DeletePropertyAsync(Guid id);

    Task<List<PropertyResponseDto>>
        GetMyPropertiesAsync(Guid agentId);

    Task<PropertyResponseDto?>
        GetMyPropertyByIdAsync(
            Guid propertyId,
            Guid agentId
        );
}