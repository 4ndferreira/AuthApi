using AuthApi.Dtos;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthApi.Contracts;

public interface IAuthService
{
  Task<Result<Guid>> RegisterAsync(RegisterRequest request);
  Task<Result<TokenResult>> LoginAsync(LoginRequest request);
}