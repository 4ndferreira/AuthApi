using AuthApi.Application.Dtos;
using AuthApi.Domain.Shared;

namespace AuthApi.Application.Abstractions;

public interface IAuthService
{
  Task<Result<Guid>> RegisterAsync(RegisterRequest request);
  Task<Result<TokenResult>> LoginAsync(LoginRequest request);
  Task<Result<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
}  