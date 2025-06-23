using AuthApi.Domain.Entities;
using AuthApi.Domain.Shared;

namespace AuthApi.Application.Abstractions;

public interface IRefreshTokenService
{
  Task<Result<TokenResult>> GenerateTokensAsync(User user);
  Task<Result<TokenResult>> RefreshTokenAsync(string refreshToken);
  Task<Result<string>> RevokeRefreshTokenAsync(string refreshToken);
}