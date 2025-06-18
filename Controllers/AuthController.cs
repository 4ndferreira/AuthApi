using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using AuthApi.Contracts.Auth;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService ?? throw new ArgumentNullException(nameof(authService));
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
  {
    var result = await _authService.RegisterAsync(request);

    return result.Success
      ? Ok(new { id = result.Value })
      : BadRequest(new { message = result.Message });
  }
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request)
  {
    var result = await _authService.LoginAsync(request);
    return result.Success
    ? Ok(result.Value) 
    : Unauthorized(new { message = result.Message });
  }
}