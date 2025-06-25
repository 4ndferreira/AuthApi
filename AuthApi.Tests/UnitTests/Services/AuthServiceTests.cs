using AuthApi.Application.Abstractions;
using AuthApi.Application.Dtos;
using AuthApi.Application.Services;
using AuthApi.Domain.Abstractions;
using AuthApi.Domain.Entities;
using AuthApi.Tests.UnitTests.Services.Builders;
using Bogus;
using Moq;

namespace AuthApi.Tests.UnitTests.Services;

public class AuthServiceTests
{
  private readonly Mock<IAuthRepository> _authRepositoryMock;
  private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
  private readonly Mock<IPasswordHasher> _passwordHasherMock;
  private readonly AuthService _authService;

  public AuthServiceTests()
  {
    _authRepositoryMock = new Mock<IAuthRepository>();
    _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
    _passwordHasherMock = new Mock<IPasswordHasher>();
    _authService = new AuthService(_authRepositoryMock.Object,
                                   _refreshTokenServiceMock.Object,
                                   _passwordHasherMock.Object);
  }

  [Fact]
  public async Task LoginAsync_ComEmailInexistente_Falha()
  {
    //Arrange
    _authRepositoryMock.Setup(x => x.GetUserByEmailAsync("inexistente@teste.com"))
                       .ReturnsAsync((User?)null);

    var request = new LoginRequest("inexistente@teste.com", "qualquer");

    //Act
    var result = await _authService.LoginAsync(request);

    //Assert
    Assert.False(result.Success);
    Assert.Equal("Email ou senha inválidos.", result.Errors!.First());
    Assert.Null(result.Value);
  }

  [Fact]
  public async Task RegisterAsync_EmailExistente_Falha()
  {
    //Arrange
    var request = RegisterRequestBuilder.Build();
    
    _authRepositoryMock.Setup(x => x.UserExistsByEmailAsync(request.Email))
                       .ReturnsAsync(true);

    //Act
    var result = await _authService.RegisterAsync(request);

    //Assert
    Assert.False(result.Success);
    Assert.Equal("Já existe usuário registrado com o email informado.", result.Errors!.First());
  }

  [Fact]
  public async Task RegisterAsync_EmailNovo_RetornaGuid()
  {
    //Arrange
    var request = RegisterRequestBuilder.Build();
    var fakeHashedPassword = "$2a$11$Fak3HashParaTestarComBCrypt";

    _passwordHasherMock.Setup(x => x.Hash(request.Password)).Returns(fakeHashedPassword);

    var fakeUser = User.CreateForRegistration(request.Email, fakeHashedPassword);

    _authRepositoryMock.Setup(x => x.UserExistsByEmailAsync(request.Email))
                       .ReturnsAsync(false);

    _authRepositoryMock.Setup(x => x.CreateUserAsync(fakeUser.Value!))
                       .Returns(Task.CompletedTask);

    //Act
    var result = await _authService.RegisterAsync(request);

    //Assert
    Assert.True(result.Success);
    Assert.NotEqual(Guid.Empty, result.Value);
    Assert.Null(result.Errors);
    Assert.NotNull(fakeUser.Value);
    Assert.Equal(request.Email, fakeUser.Value!.Email);
    Assert.Equal(fakeHashedPassword, fakeUser.Value!.PasswordHash);
  }
}     