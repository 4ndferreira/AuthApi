using AuthApi.Contracts.Auth;
using AuthApi.Contracts.Token;
using AuthApi.Dtos;
using AuthApi.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthApi.Service;

public class AuthService : IAuthService
{
  private readonly IAuthRepository _authRepository;
  private readonly ITokenService _tokenService;

  public AuthService(IAuthRepository authRepository, ITokenService tokenService)
  {
    _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
    _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
  }

  public async Task<Result<TokenResult>> LoginAsync(LoginRequest request)
  {
    var user = await _authRepository.GetUserByEmailAsync(request.Email);

    if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
      return Result<TokenResult>.Failure("Email ou senha inválidos.");

    var token = _tokenService.GenerateToken(user);
    
    return Result<TokenResult>.SuccessResult(token);
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