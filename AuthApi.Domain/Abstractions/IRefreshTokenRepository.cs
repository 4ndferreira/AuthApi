using AuthApi.Domain.Entities;

namespace AuthApi.Domain.Abstractions;

public interface IRefreshTokenRepository
{
  Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
  Task GenerateRefreshTokenAsync(RefreshToken refreshToken);
  Task RevokeRefreshTokenAsync(string refreshToken);
}