using Moq;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Application.Services.Auth.Query;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Tests.Services.Auth;

public class LoginQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Token_When_Credentials_Are_Correct()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var password = "myPassword";
        var salt = "mySalt";
        var hashedPassword = "hashedPass";
        var token = "jwt-token";

        var request = new LoginRequest
        {
            UserName = "testuser",
            Password = password
        };

        var user = User.CreateUser(userId, request.UserName, "test@example.com", hashedPassword, salt);

        var userRepoMock = new Mock<IUserQueryRepository>();
        var encryptionMock = new Mock<IEncryptionUtility>();

        userRepoMock.Setup(u => u.Login(request.UserName, request.Password)).ReturnsAsync(user);
        encryptionMock.Setup(e => e.GetSHA256(password, salt)).Returns(hashedPassword);
        encryptionMock.Setup(e => e.GetNewToken(userId)).Returns(token);

        var handler = new LoginQueryHandler(userRepoMock.Object, encryptionMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(request.UserName, result.UserName);
        Assert.Equal(token, result.Token);
        
        userRepoMock.Verify(u => u.Login(request.UserName, request.Password), Times.Once);
        encryptionMock.Verify(e => e.GetSHA256(password, salt), Times.Once);
        encryptionMock.Verify(e => e.GetNewToken(userId), Times.Once);
    }
}