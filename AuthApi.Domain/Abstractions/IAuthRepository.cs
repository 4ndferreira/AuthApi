using AuthApi.Domain.Entities;

namespace AuthApi.Domain.Abstractions;

public interface IAuthRepository
{
  Task<bool> UserExistsByEmailAsync(string email);
  Task CreateUserAsync(User user);
  Task<User?> GetUserByEmailAsync(string email); 
}