namespace IRCM.DTOs.Auth;

public class AuthResponseDto
{

    public required string Token { get; set; }
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Role { get; set; }

    public DateTime CreatedAt { get; set; }
}