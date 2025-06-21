namespace AuthApi.Application.Settings;

public class JwtSettings
{
  public required string Key { get; set; }
  public string Issuer { get; set; } = "AuthApi";
  public string Audience { get; set;} = "AuthApiUser";
  public int ExpireMinutes { get; set; } = 60;
}