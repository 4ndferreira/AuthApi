using AuthApi.Domain.Shared;

namespace AuthApi.Domain.Entities;

public sealed class User : Entity
{
  public string? Name { get; private set;}
  public string? Username { get; private set; }
  public string Email { get; private set; }
  public string PasswordHash { get; private set; }
  public string Role { get; private set; } = "User";

  private User(string email, string passwordHash, string? name = null, string? username = null)
  {
    Name = name;
    Username = username;
    Email = email;
    PasswordHash = passwordHash;
  }

  public static Result<User> CreateForRegistration(string email, string passwordHash)
  {
    var user = new User(email, passwordHash);

    var errors = user.Validate(true);
    
    return errors.Count != 0
      ? Result<User>.Failure(errors)
      : Result<User>.SuccessResult(user);
  }

  public static Result<User> UpdateProfile(string username, string name, string role)
  {
    var user = new User(username, name, role);

    var errors = user.Validate(false);

    return errors.Count != 0
      ? Result<User>.Failure(errors)
      : Result<User>.SuccessResult(user);
  }

  public Result UpdatePassword(string newPasswordHashed)
  {
    if (string.IsNullOrWhiteSpace(newPasswordHashed))
      return Result.Failure("A senha não pode ser vazia.");

    PasswordHash = newPasswordHashed;
    return Result.SuccessResult();
  }

  private List<string> Validate(bool isRegistration)
  {
    var errors = new List<string>();
    if (string.IsNullOrWhiteSpace(Email))
      errors.Add("O campo Email é obrigatório");
    if (string.IsNullOrWhiteSpace(PasswordHash))
      errors.Add("O campo Senha é obrigatório.");

    if (!isRegistration)
    {
      if (string.IsNullOrWhiteSpace(Username))
        errors.Add("O campo Username não pode ser vazio");
      if (string.IsNullOrWhiteSpace(Name))
        errors.Add("O campo Name não pode ser vazio");
    }

    return errors;
  }
}