using IRCM.Enums;

namespace IRCM.DTOs.Auth;

public class RegisterDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    //if role method is implemented, this will be used to assign role during 
    // registration if not it will default to tenant in the user model
    public UserRole? Role { get; set; } = UserRole.Tenant;
}