namespace AuthApi.Settings;

public sealed record class JwtSettings
{
  public required string Key { get; init; }
  public string Issuer { get; init; } = "AuthApi";
  public string Audience { get; init;} = "AuthApiUser";
  public int ExpireMinutes { get; init; } = 60;
}