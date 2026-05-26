namespace IRCM.DTOs.Auth;

public class AuthResponseDto
{

    public required string Token { get; set; }
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Role { get; set; }    = string.Empty;

    public DateTime CreatedAt { get; set; }
}