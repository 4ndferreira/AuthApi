using AuthApi.Contracts;
using AuthApi.Dtos;
using AuthApi.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthApi.Services;

public class AuthService : IAuthService
{
  private readonly IAuthRepository _authRepository;
  private readonly IRefreshTokenService _refreshTokenService;

  public AuthService(IAuthRepository authRepository, IRefreshTokenService refreshTokenService)
  {
    _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
    _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
  }

  public async Task<Result<TokenResult>> LoginAsync(LoginRequest request)
  {
    var user = await _authRepository.GetUserByEmailAsync(request.Email);

    if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
      return Result<TokenResult>.Failure("Email ou senha inválidos.");

    var tokens = await _refreshTokenService.GenerateTokensAsync(user);
    
    return Result<TokenResult>.SuccessResult(tokens.Value!);
  }

  public async Task<Result<Guid>> RegisterAsync(RegisterRequest request)
  {
    var existingUser = await _authRepository.UserExistsByEmailAsync(request.Email);

    if (existingUser)
      return Result<Guid>.Failure("Já existe usuário registrado com o email informado.");

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

    var user = new User(request.Email, hashedPassword);

    await _authRepository.CreateUserAsync(user);
    
    return Result<Guid>.SuccessResult(user.Id);
  }
}