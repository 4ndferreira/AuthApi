using AuthApi.Models;
using AuthApi.Dtos;

namespace AuthApi.Contracts;

public interface ITokenService
{
  TokenResult GenerateToken(User user);
}