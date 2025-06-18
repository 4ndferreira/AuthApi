namespace AuthApi.Dtos;

public class TokenResult
{
  public required string AccessToken { get; init; }
  public DateTime Expiration { get; init; }
}