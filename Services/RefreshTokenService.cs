using System.Security.Cryptography;
using AuthApi.Contracts;
using AuthApi.Dtos;
using AuthApi.Models;

namespace AuthApi.Services;

public class RefreshTokenService : IRefreshTokenService
{
  private readonly IRefreshTokenRepository _refreshTokenRepository;
  private readonly ITokenService _tokenService;

  public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
  {
    _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
  }
  public async Task<Result<TokenResult>> GenerateTokensAsync(User user)
  {
    var tokens = _tokenService.GenerateToken(user);
    var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    var expiresAt = DateTime.UtcNow.AddDays(7);

    var entity= new RefreshToken
    {
      Token = refreshToken,
      ExpiresAt = expiresAt,
      UserId = user.Id,
    };

    tokens.RefreshToken = refreshToken;

    await _refreshTokenRepository.GenerateRefreshTokenAsync(entity);

    return Result<TokenResult>.SuccessResult(tokens);
  }

  public async Task<Result<TokenResult>> RefreshTokenAsync(string refreshToken)
  {
    var tokenFromDb = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
    if (tokenFromDb == null || tokenFromDb.IsRevoked)
      return Result<TokenResult>.Failure("Refresh Token inválido ou expirado.");
    
    var user = tokenFromDb.User;
    var newTokens = await GenerateTokensAsync(user!);

    await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);

    return Result<TokenResult>.SuccessResult(newTokens.Value!);
  }

  public async Task RevokeRefreshTokenAsync(string refreshToken)
  {
    await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);
  }
}