using AuthApi.Application.Dtos;
using Bogus;

namespace AuthApi.Tests.UnitTests.Services.Builders;
public static class RegisterRequestBuilder
{
  public static RegisterRequest Build()
  {
    var registerRequestFaker = new Faker<RegisterRequest>()
      .RuleFor("Email", f => f.Internet.Email())
      .RuleFor("Password", f => f.Internet.Password());

    return registerRequestFaker.Generate();
  }
}