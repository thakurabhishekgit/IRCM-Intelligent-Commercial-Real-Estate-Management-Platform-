using IRCM.DTOs.Auth;
using IRCM.DTOs.User;

namespace IRCM.Interfaces;

public interface IUserService
{


    Task<List<UserResponseDto>> GetAllUsersAsync();

    Task<UserResponseDto?> GetUserByIdAsync(Guid id);
}