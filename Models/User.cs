namespace AuthApi.Models;

public class User
{
  public Guid Id { get; set; }
  //public string? Name { get; set;} = null;
  //public string? Username { get; set; } = null;
  public string? Email { get; set; } = null;
  public string? PasswordHash { get; set; } = null;
  //public string? Role { get; set; }
  public DateTime CreateAt { get; set; } = DateTime.UtcNow;

  public User(string email, string passwordHash)
  {
    Email = email;
    PasswordHash = passwordHash;
  }
}