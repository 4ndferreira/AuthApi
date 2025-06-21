namespace AuthApi.Application.Dtos;
public sealed record UserProfileDto(
  string UserId,
  string Email,
  string? Name,
  string? Username,
  string? Role
);