namespace AuthApi.Application.Dtos;

public class TokenResult
{
  public required string AccessToken { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime Expiration { get; set; }
}