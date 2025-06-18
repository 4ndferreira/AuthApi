using AuthApi.Contracts.Auth;
using AuthApi.Data;
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Repository;

public class AuthRepository : IAuthRepository
{
  private readonly AppDbContext _context;

  public AuthRepository(AppDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }
  public async Task CreateUserAsync(User user)
  {
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
  }

  public async Task<User> GetUserByEmailAsync(string email)
  {
    var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
    return user;
  }

  public async Task<bool> UserExistsByEmailAsync(string email)
  {
    return await _context.Users.AnyAsync(u => u.Email == email);
  }
}