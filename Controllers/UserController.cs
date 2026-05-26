using IRCM.DTOs.User;
using IRCM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRCM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET ALL USERS

    [HttpGet]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(new
        {
            success = true,
            message = "Users fetched successfully",
            data = users
        });
    }

    // GET USER BY ID

    [HttpGet("{id}")]
    // [Authorize]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "User fetched successfully",
            data = user
        });
    }


    // UPDATE USER
    [HttpPut("{id}")]
    // [Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDtoRequest dto)
    {
        var user = await _userService.UpdateUserAsync(id, dto);

        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "User updated successfully",
            data = user
        });
    }
}