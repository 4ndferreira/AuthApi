using AuthApi.Domain.Entities;
using AuthApi.Application.Dtos;

namespace AuthApi.Application.Abstractions;

public interface IJwtTokenGenerator
{
  TokenResult GenerateToken(User user);
}