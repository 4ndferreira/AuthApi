using AuthApi.Contracts;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Identity.Data;
using Moq;
using Xunit;

namespace AuthApi.Tests.Services;

public class AuthServiceTests
{
  private readonly Mock<IAuthRepository> _authRepositoryMock;
  private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
  private readonly AuthService _authService;

  public AuthServiceTests()
  {
    _authRepositoryMock = new Mock<IAuthRepository>();
    _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
    _authService = new AuthService(_authRepositoryMock.Object, _refreshTokenServiceMock.Object);
  }

  [Fact]
  public async Task LoginAsync_ComEmailInexistente_Falha()
  {
    //Arrange
    _authRepositoryMock.Setup(x => x.GetUserByEmailAsync("inexistente@teste.com"))
                       .ReturnsAsync((User?)null);

    var request = new LoginRequest { Email = "inexistente@teste.com", Password = "qualquer" };

    //Act
    var result = await _authService.LoginAsync(request);

    //Assert
    Assert.False(result.Success);
    Assert.Equal("Email ou senha inválidos.", result.Message);
    Assert.Null(result.Value);
  }

  [Fact]
  public async Task RegisterAsync_EmailExistente_Falha()
  {
    //Arrange
    _authRepositoryMock.Setup(x => x.UserExistsByEmailAsync("existente@teste.com"))
                       .ReturnsAsync(true);

    var request = new RegisterRequest { Email = "existente@teste.com", Password = "qualquer" };

    //Act
    var result = await _authService.RegisterAsync(request);

    //Assert
    Assert.False(result.Success);
    Assert.Equal("Já existe usuário registrado com o email informado.", result.Message);
  }

  [Fact]
  public async Task RegisterAsync_EmailNovo_RetornaGuid()
  {
    //Arrange
    _authRepositoryMock.Setup(x => x.UserExistsByEmailAsync("existente@teste.com"))
                       .ReturnsAsync(false);

    _authRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                       .Callback<User>(user => user.Id = Guid.NewGuid());

    var request = new RegisterRequest { Email = "existente@teste.com", Password = "senha_valida" };

    //Act
    var result = await _authService.RegisterAsync(request);

    //Assert
    Assert.True(result.Success);
    Assert.NotEqual(Guid.Empty, result.Value);
    Assert.Null(result.Message);
  }
}     