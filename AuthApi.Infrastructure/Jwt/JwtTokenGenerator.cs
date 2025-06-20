using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApi.Application.Abstractions;
using AuthApi.Application.Dtos;
using AuthApi.Domain.Entities;
using AuthApi.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Infrastructure.Jwt;

public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly JwtSettings _jwt;

  public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
  {
    _jwt = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions), "JWT config ausente");

    if (string.IsNullOrWhiteSpace(_jwt.Key))
      throw new ArgumentException("A chave JWT está vazia ou é inválida.", nameof(jwtOptions));
  }

  public TokenResult GenerateToken(User user)
  {
    var claims = GetClaims(user);
    var credentials = GetSigningCredentials();
    var token = CreateJwtToken(claims, credentials);

    return new TokenResult
    {
      AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
      Expiration = token.ValidTo
    };
  }

  private static Claim[] GetClaims(User user)
  {
    return
    [
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email!),
      new Claim(ClaimTypes.Email, user.Email!),
      new Claim(ClaimTypes.Name, user.Name!),
      new Claim("username", user.Username!),
      new Claim(ClaimTypes.Role, user.Role!),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    ];
  }

  private SigningCredentials GetSigningCredentials()
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
    return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
  }

  private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials)
  {
    return new JwtSecurityToken(
      issuer: _jwt.Issuer,
      audience: _jwt.Audience,
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes),
      signingCredentials: credentials
    );
  }
}