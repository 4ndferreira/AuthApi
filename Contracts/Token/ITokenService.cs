using AuthApi.Models;

namespace AuthApi.Contracts.Token;

public interface ITokenService
{
  string GenerateToken(User user);
}