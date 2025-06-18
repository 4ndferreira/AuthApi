using AuthApi.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthApi.Models;
using AuthApi.Service;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly AppDbContext _context;
  private readonly TokenService _tokenService;

  public AuthController(AppDbContext context, TokenService tokenService)
  {
    _context = context;
    _tokenService = tokenService;
  }
  
  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
  {
    var existingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);

    if (existingUser)
      return BadRequest("Já existe usuário registrado com o email informado.");

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

    var user = new User
    {
      Email = request.Email,
      PasswordHash = hashedPassword
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok(new { id = user.Id });
  }
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
    {
      return Unauthorized(new { message = "Email ou senha inválidos" });
    }

    var token = _tokenService.GenerateToken(user);
    return Ok(new { token });
  }
}