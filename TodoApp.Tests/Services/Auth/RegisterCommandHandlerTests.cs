using Moq;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Application.Services.Auth.Command;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Tests.Services.Auth;

public class RegisterCommandHandlerTests
{
    private Mock<IEncryptionUtility> _encryptionMock = new();
    private Mock<IUserCommandRepository> _userRepoMock = new();
    private Mock<IUnitOfWork> _unitOfWorkMock = new();
    
    private string _salt = Guid.NewGuid().ToString();
    private string _hashedPassword = "passwordSalt";
    private string _password = "password";
    
    private RegisterCommandHandler CreateHandler() =>
        new(_encryptionMock.Object, _userRepoMock.Object, _unitOfWorkMock.Object);
    
    private RegisterRequest CreateRequest(string username = "testuser", string email = "test@example.com", string password = "pass123")
        => new() { UserName = username, Email = email, Password = password };
    
    [Fact]
    public async Task Handle_UserInfoTrue_ShouldCreateUser()
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(password: _password);
        await CreateHandler().Handle(request, CancellationToken.None);
        
        _userRepoMock.Verify(r => r.CreateUser(It.Is<User>(
            u => u.UserName == request.UserName && u.Email == request.Email)), Times.Once);
        
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_UserWhenUsernameNullOrEmptyString_ShouldThrowNotValidException(string invalidUserName)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(username: invalidUserName);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Username cannot be empty", ex.Message);
        
        _userRepoMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_UserWhenEmailNullOrEmptyString_ShouldThrowNotValidException(string invalidEmail)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(email: invalidEmail);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Email cannot be empty", ex.Message);
        
        _userRepoMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_UserWhenPasswordNullOrEmptyString_ShouldThrowNotValidException(string invalidPassword)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(password: invalidPassword);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Password cannot be empty", ex.Message);
        
        _userRepoMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}