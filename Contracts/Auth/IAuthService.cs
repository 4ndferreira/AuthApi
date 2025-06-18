using AuthApi.Dtos;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthApi.Contracts.Auth;

public interface IAuthService
{
  Task<RegisterResult> RegisterAsync(RegisterRequest request);
}