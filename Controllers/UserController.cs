using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  [HttpGet("profile"), Authorize]
  public IActionResult GetProfile()
  {
    var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var email = User.FindFirstValue(ClaimTypes.Email);

    return Ok( new
    {
      UserId = id,
      Email = email,
      Message = "Você está autenticado!"
    });
  }
}
