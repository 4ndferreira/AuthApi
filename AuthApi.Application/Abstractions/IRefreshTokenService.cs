using AuthApi.Application.Dtos;
using AuthApi.Domain.Entities;

namespace AuthApi.Application.Abstractions;

public interface IRefreshTokenService
{
  Task<Result<TokenResult>> GenerateTokensAsync(User user);
  Task<Result<TokenResult>> RefreshTokenAsync(string refreshToken);
  Task RevokeRefreshTokenAsync(string refreshToken);
}