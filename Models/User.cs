namespace AuthApi.Models;

public class User
{
  public Guid Id { get; set; }
  public string? Name { get; set;}
  public string? Username { get; set; }
  public string? Email { get; set; }
  public string? PasswordHash { get; set; }
  public string? Role { get; set; }
  public DateTime CreateAt { get; set; }

  public User(string email, string passwordHash)
  {
    Email = email;
    PasswordHash = passwordHash;
    CreateAt = DateTime.UtcNow;
  }
}