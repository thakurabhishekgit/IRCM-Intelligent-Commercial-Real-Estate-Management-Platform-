using IRCM.Data;
using IRCM.DTOs.Auth;
using IRCM.Interfaces;
using IRCM.Models;
using Microsoft.EntityFrameworkCore;
using IRCM.DTOs.User;
using IRCM.Enums;
using IRCM.Helpers;
namespace IRCM.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
    ApplicationDbContext context,
    JwtHelper jwtHelper
)
{
    _context = context;
    _jwtHelper = jwtHelper;
}

    // REGISTER

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (existingUser != null)
        {
            return null;
        }
        
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role ?? UserRole.Tenant
        };
        var token = _jwtHelper.GenerateToken(user);
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        return new AuthResponseDto
{
    Id = user.Id,
    FullName = user.FullName,
    Email = user.Email,
    PhoneNumber = user.PhoneNumber,
    Role = user.Role.ToString(),
    Token = token,
    CreatedAt = user.CreatedAt
};
    }

    // LOGIN

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null)
        {
            return null;
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!isPasswordValid)
        {
            return null;
        }

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Token = token,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<AuthResponseDto?> createAgentWithAdminRoleAsync(RegisterDto dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (existingUser != null)
        {
            return null;
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Agent
        };

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Token = token,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

   
}