using System.Security.Cryptography;
using AuthApi.Application.Abstractions;
using AuthApi.Domain.Abstractions;
using AuthApi.Domain.Entities;
using AuthApi.Domain.Shared;

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
    
    var entity = RefreshToken.Create(refreshToken, user.Id);

    tokens.RefreshToken = refreshToken;

    await _refreshTokenRepository.GenerateRefreshTokenAsync(entity.Value!);

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

  public async Task<Result<string>> RevokeRefreshTokenAsync(string refreshToken)
  {
    var tokenFromDb = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
    if (tokenFromDb == null || tokenFromDb.IsRevoked)
      return Result<string>.Failure("Refresh Token inválido ou já revogado.");

    await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);

    return Result<string>.SuccessResult("Logout realizado com sucesso.");
  }
}