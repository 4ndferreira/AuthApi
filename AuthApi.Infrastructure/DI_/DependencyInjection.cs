using AuthApi.Application.Abstractions;
using AuthApi.Application.Services;
using AuthApi.Domain.Abstractions;
using AuthApi.Infrastructure.Data;
using AuthApi.Infrastructure.Jwt;
using AuthApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApi.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
  {
    services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=auth.db"));

    services.AddScoped<IAuthRepository, AuthRepository>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    services.AddScoped<IRefreshTokenService, RefreshTokenService>();
    services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

    return services;
  }
}