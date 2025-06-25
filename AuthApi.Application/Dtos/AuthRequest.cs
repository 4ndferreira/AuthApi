namespace AuthApi.Application.Dtos;

public sealed record RegisterRequest
{
  public required string Email { get; init; }
  public required string Password { get; init; }  
};
public sealed record LoginRequest(string Email, string Password);
public sealed record RefreshRequest(string RefreshToken);
public sealed record LogoutRequest(string RefreshToken);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);