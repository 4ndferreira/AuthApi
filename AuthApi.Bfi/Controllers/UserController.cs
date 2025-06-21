using AuthApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.Bfi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
  [HttpGet("profile")]
  [Authorize]
  [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public IActionResult GetProfile()
  {
    var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var email = User.FindFirstValue(ClaimTypes.Email);
    var name = User.FindFirstValue(ClaimTypes.Name);
    var username = User.FindFirst("username")?.Value;
    var role = User.FindFirstValue(ClaimTypes.Role);

    var profile = new UserProfileDto(id!, email!, name, username, role);

    return Ok(profile);
  }
}
