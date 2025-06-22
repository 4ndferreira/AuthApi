using AuthApi.Application.Abstractions;
using AuthApi.Application.Dtos;
using AuthApi.Domain.Abstractions;
using AuthApi.Domain.Entities;

namespace AuthApi.Application.Services;

public class AuthService : IAuthService
{
  private readonly IAuthRepository _authRepository;
  private readonly IRefreshTokenService _refreshTokenService;
  private readonly IPasswordHasher _passwordHasher;

  public AuthService(IAuthRepository authRepository, IRefreshTokenService refreshTokenService, IPasswordHasher passwordHasher)
  {
    _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
    _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
    _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
  }

  public async Task<Result<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
  {
    var user = await _authRepository.GetUserByIdAsync(userId);
    if (user == null) 
      return Result<string>.Failure("Usuário não encontrado.");

    if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
      return Result<string>.Failure("Senha atual incorreta.");

    if (request.CurrentPassword == request.NewPassword)
      return Result<string>.Failure("A nova senha não pode ser igual à atual.");

    user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
    await _authRepository.SaveChangesAsync(user);

    return Result<string>.SuccessResult("Senha alterada com sucesso.");
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