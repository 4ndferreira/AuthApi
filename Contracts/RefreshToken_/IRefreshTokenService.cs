using AuthApi.Dtos;
using AuthApi.Models;

namespace AuthApi.Contracts;

public interface IRefreshTokenService
{
  Task<Result<TokenResult>> GenerateTokensAsync(User user);
  Task<Result<TokenResult>> RefreshTokenAsync(string refreshToken);
  Task RevokeRefreshTokenAsync(string refreshToken);
}