using AuthApi.Domain.Entities;
using AuthApi.Domain.Shared;

namespace AuthApi.Application.Abstractions;

public interface IJwtTokenGenerator
{
  TokenResult GenerateToken(User user);
}