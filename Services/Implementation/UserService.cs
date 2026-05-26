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
}