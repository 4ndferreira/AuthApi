
using AuthApi.Models;

namespace AuthApi.Contracts;

public interface IRefreshTokenRepository
{
  Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
  Task GenerateRefreshTokenAsync(RefreshToken refreshToken);
  Task RevokeRefreshTokenAsync(string refreshToken);
}