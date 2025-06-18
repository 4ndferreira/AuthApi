using AuthApi.Models;
using AuthApi.Dtos;

namespace AuthApi.Contracts.Token;

public interface ITokenService
{
  TokenResult GenerateToken(User user);
}