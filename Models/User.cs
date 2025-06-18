namespace AuthApi.Models;

public class User
{
  public Guid Id { get; set; }
  public string? Email { get; set; } = null;
  public string? PasswordHash { get; set; } = null;
  public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}