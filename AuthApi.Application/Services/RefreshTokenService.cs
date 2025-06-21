using System.Security.Cryptography;
using AuthApi.Application.Abstractions;
using AuthApi.Application.Dtos;
using AuthApi.Domain.Abstractions;
using AuthApi.Domain.Entities;

namespace AuthApi.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
  private readonly IRefreshTokenRepository _refreshTokenRepository;
  private readonly IJwtTokenGenerator _jwtTokenGenerator;

  public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IJwtTokenGenerator tokenService)
  {
    _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    _jwtTokenGenerator = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
  }
  public async Task<Result<TokenResult>> GenerateTokensAsync(User user)
  {
    var tokens = _jwtTokenGenerator.GenerateToken(user);
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