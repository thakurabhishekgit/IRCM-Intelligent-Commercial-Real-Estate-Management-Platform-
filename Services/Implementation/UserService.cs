using IRCM.Data;
using IRCM.DTOs.Auth;
using IRCM.Interfaces;
using IRCM.Models;
using Microsoft.EntityFrameworkCore;
using IRCM.DTOs.User;
namespace IRCM.Services.Implementation;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // REGISTER



    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(x => new UserResponseDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Role = x.Role.ToString(),
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    // GET USER BY ID
    
    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users
            .Where(x => x.Id == id)
            .Select(x => new UserResponseDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Role = x.Role.ToString(),
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserResponseDto?> UpdateUserAsync(Guid id, UpdateUserDtoRequest dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return null;
        }

        user.FullName = dto.FullName ?? user.FullName;
        user.Email = dto.Email ?? user.Email;
        user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}