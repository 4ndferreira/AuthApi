namespace AuthApi.Domain.Entities;

public class RefreshToken
{
  public Guid Id { get; set; }
  public string Token { get; set; } = string.Empty;
  public DateTime ExpiresAt { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? RevokedAt { get; set; }
  public bool IsRevoked => RevokedAt != null || DateTime.UtcNow > ExpiresAt;

  public Guid UserId { get; set; }
  public User? User { get; set; }
}