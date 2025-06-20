using AuthApi.Models;

namespace AuthApi.Contracts;

public interface IAuthRepository
{
  Task<bool> UserExistsByEmailAsync(string email);
  Task CreateUserAsync(User user);
  Task<User?> GetUserByEmailAsync(string email); 
}