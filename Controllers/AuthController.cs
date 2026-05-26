using IRCM.DTOs.Auth;
using IRCM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IRCM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // REGISTER

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);

        if (user == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "User already exists"
            });
        }

        return StatusCode(201, new
        {
            success = true,
            message = "User registered successfully",
            data = user
        });
    }

    // LOGIN

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _authService.LoginAsync(dto);

        if (user == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Login successful",
            data = user
        });
    }

    // GET ALL USERS


    // GET USER BY ID

  
}