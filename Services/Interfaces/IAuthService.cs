using IRCM.DTOs.Auth;
using IRCM.DTOs.User;
namespace IRCM.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);

    Task<AuthResponseDto?> LoginAsync(LoginDto dto);

    Task<AuthResponseDto?> createAgentWithAdminRoleAsync(RegisterDto dto);

}