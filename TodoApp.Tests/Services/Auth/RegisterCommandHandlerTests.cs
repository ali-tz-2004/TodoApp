using Moq;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Application.Services.Auth.Command;
using TodoApp.Common;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Tests.Services.Auth;

public class RegisterCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_User_And_Save()
    {
        // Arrange
        var encryptionMock = new Mock<IEncryptionUtility>();
        var userRepoMock = new Mock<IUserCommandRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var salt = Guid.NewGuid().ToString();
        var hashedPassword = "password" + salt;
        var password = "password";

        encryptionMock.Setup(e => e.GetNewSalt()).Returns(salt);
        encryptionMock.Setup(e => e.GetSHA256(password, salt)).Returns(hashedPassword);

        var request = new RegisterRequest
        {
            UserName = "testuser",
            Email = "test@example.com",
            Password = "pass123"
        };

        var handler = new RegisterCommandHandler(
            encryptionMock.Object,
            userRepoMock.Object,
            unitOfWorkMock.Object);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        userRepoMock.Verify(r => r.CreateUser(It.Is<User>(
            u => u.UserName == request.UserName && u.Email == request.Email)), Times.Once);

        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}