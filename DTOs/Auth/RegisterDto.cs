namespace IRCM.DTOs.Auth;

public class RegisterDto
{
    public string FullName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    //if role method is implemented, this will be used to assign role during 
    // registration if not it will default to tenant in the user model
    public string Role { get; set; }
}