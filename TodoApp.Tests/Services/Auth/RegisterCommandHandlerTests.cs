using Moq;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Tests.Services.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IEncryptionUtility> _encryptionMock = new();
    private readonly Mock<IUserCommandRepository> _userCommandRepositoryMock = new();
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    
    private readonly string _salt = Guid.NewGuid().ToString();
    private readonly string _hashedPassword = "passwordSalt";
    private readonly string _password = "password";
    
    private RegisterCommandHandler CreateHandler() =>
        new(_encryptionMock.Object, _userCommandRepositoryMock.Object, _userQueryRepositoryMock.Object, _unitOfWorkMock.Object);
    
    private RegisterCommand CreateRequest(string username = "testuser", string email = "test@example.com", string password = "pass123")
        => new() { UserName = username, Email = email, Password = password };
    
    [Fact]
    public async Task user_information_is_valid()
    {
        var request = CreateRequest(password: _password);

        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        _userQueryRepositoryMock.Setup(x=>x.ExistsByUserName(request.UserName, CancellationToken.None)).ReturnsAsync(false);
        _userQueryRepositoryMock.Setup(x=>x.ExistsByEmail(request.Email, CancellationToken.None)).ReturnsAsync(false);

        await CreateHandler().Handle(request, CancellationToken.None);
        
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.Is<User>(
            u => u.UserName == request.UserName && u.Email == request.Email)), Times.Once);
        
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task when_username_is_null_or_empty_string(string invalidUserName)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(username: invalidUserName);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Username cannot be empty", ex.Message);
        
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task when_email_is_null_or_empty_string(string invalidEmail)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(email: invalidEmail);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Email cannot be empty", ex.Message);
        
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task when_password_is_null_or_empty_string(string invalidPassword)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(password: invalidPassword);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Password cannot be empty", ex.Message);
        
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData("userExample.com")]
    [InlineData("user@Examplecom")]
    public async Task when_email_wrong_pattern(string wrongEmail)
    {
        _encryptionMock.Setup(e => e.GetNewSalt()).Returns(_salt);
        _encryptionMock.Setup(e => e.GetSHA256(_password, _salt)).Returns(_hashedPassword);

        var request = CreateRequest(email: wrongEmail, password: _password);

        var ex = await Assert.ThrowsAsync<NotValidException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Email is not valid", ex.Message);
        
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task when_userName_is_conflict()
    {
        var request = CreateRequest();
        
        _userQueryRepositoryMock.Setup(x=>x.ExistsByUserName(request.UserName, CancellationToken.None)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("UserName already exists", ex.Message);
        
        _encryptionMock.Verify(r => r.GetNewSalt(), Times.Never);
        _encryptionMock.Verify(u => u.GetSHA256(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task when_email_is_conflict()
    {
        var request = CreateRequest();
        
        _userQueryRepositoryMock.Setup(x=>x.ExistsByUserName(request.UserName, CancellationToken.None)).ReturnsAsync(false);
        _userQueryRepositoryMock.Setup(x=>x.ExistsByEmail(request.Email, CancellationToken.None)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
            CreateHandler().Handle(request, CancellationToken.None));
        
        Assert.Contains("Email already exists", ex.Message);
        
        _encryptionMock.Verify(r => r.GetNewSalt(), Times.Never);
        _encryptionMock.Verify(u => u.GetSHA256(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _userCommandRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}