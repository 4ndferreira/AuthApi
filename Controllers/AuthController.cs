using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using AuthApi.Contracts;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;
  private readonly IRefreshTokenService _refreshTokenService;

  public AuthController(IAuthService authService, IRefreshTokenService refreshTokenService)
  {
    _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
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
  [HttpPost("refresh")]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
  {
    var result = await _refreshTokenService.RefreshTokenAsync(request.RefreshToken);
    return result.Success
    ? Ok(new { result.Value!.AccessToken, result.Value.RefreshToken}) 
    : Unauthorized(new { message = result.Message });
  }
}