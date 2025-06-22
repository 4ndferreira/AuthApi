using AuthApi.Application.Abstractions;
using AuthApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.Bfi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
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
  [HttpPost("logout")]
  [Authorize]
  public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
  {
    var result = await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);

    return result.Success
    ? Ok(new { message = result.Value })
    : BadRequest(new { message = result.Message });
  }
  [HttpPost("change-password")]
  [Authorize]
  public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
  {
    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!Guid.TryParse(userIdClaim, out var userId))
      return Unauthorized();

    var result = await _authService.ChangePasswordAsync(userId, request);

    return result.Success
    ? NoContent()
    : BadRequest(new { message = result.Message });
  }
}