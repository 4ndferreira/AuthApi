using AuthApi.Domain.Entities;
using Bogus;

public static class UserBuilder
{
  public static User Build()
  {
    var userFaker = new Faker<User>()
      .RuleFor("Email", f => f.Internet.Email());

    return userFaker;
  }
}