using AuthApi.Domain.Abstractions;
using AuthApi.Infrastructure.Data;
using AuthApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
  private readonly AppDbContext _context;
  public RefreshTokenRepository(AppDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
  {
    return await _context.RefreshTokens.Include(t => t.User)
                                       .FirstOrDefaultAsync(t => t.Token == refreshToken);
  }
  public async Task GenerateRefreshTokenAsync(RefreshToken refreshToken)
  {
    _context.RefreshTokens.Add(refreshToken);
    await _context.SaveChangesAsync();
  }
  public async Task RevokeRefreshTokenAsync(string refreshToken)
  {
    var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
    if (token != null && !token.IsRevoked)
    {
      token.RevokedAt = DateTime.UtcNow;
      await _context.SaveChangesAsync(); 
    }
  }
}