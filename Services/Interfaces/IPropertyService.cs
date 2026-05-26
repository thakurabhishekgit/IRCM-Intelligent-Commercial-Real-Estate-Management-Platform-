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

    Task<PropertyResponseDto?> UpdatePropertyAsync(
        Guid id,
        UpdatePropertyDto dto
    );

    Task<bool> DeletePropertyAsync(Guid id);
}