using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SrDoc.Api.DTOs;
using System.Security.Claims;

namespace SrDoc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Apenas usuários logados acessam este controller
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserResponseDTO>> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        return Ok(new UserResponseDTO(user.Id, user.Email!, user.UserName!));
    }
}