using AuthApi.Domain.Shared;

namespace AuthApi.Domain.Entities;

public sealed class RefreshToken : Entity
{
  public string Token { get; private set; } = string.Empty;
  public DateTime ExpiresAt { get; private set; }
  public DateTime? RevokedAt { get; private set; }
  public bool IsRevoked => RevokedAt != null || DateTime.UtcNow > ExpiresAt;

  public Guid UserId { get; private set; }
  public User? User { get; private set; }

  private RefreshToken(string token, Guid userId)
  {
    Token = token;
    ExpiresAt = DateTime.UtcNow.AddDays(7);
    UserId = userId;
  }

  public static Result<RefreshToken> Create(string token, Guid userId)
  {
    var refreshToken = new RefreshToken(token, userId);

    var errors = refreshToken.Validate();

    return errors.Count != 0
      ? Result<RefreshToken>.Failure(errors)
      : Result<RefreshToken>.SuccessResult(refreshToken);
  }

  public void RevokeToken()
  {
    RevokedAt = DateTime.UtcNow;
  }

  private List<string> Validate()
  {
    var errors = new List<string>();
    if (string.IsNullOrWhiteSpace(Token))
      errors.Add("O Token é obrigatório");
    if (UserId == Guid.Empty)
      errors.Add("Nenhum usuário associado.");

    return errors;
  }
}