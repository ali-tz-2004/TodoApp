using Moq;
using TodoApp.Application.Auth.Commands.LoginCommand;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Tests.Services.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock = new();
    private readonly Mock<IEncryptionUtility> _encryptionUtilityMock = new();

    private readonly string _username = "user";
    private readonly string _email = "user";
    private readonly string _password = "123";
    private readonly string _salt = "random_salt";
    private readonly string _hashedPassword = "hashed_123";
    private readonly string _token = "fake_token";
    
    private LoginCommand _request() => new()
    {
        UserName = _username,
        Password = _password
    };

    [Fact]
    public async Task when_user_not_found()
    {
        _userQueryRepositoryMock.Setup(x => x.GetByUserName(_username, CancellationToken.None))
            .Throws(new NotFoundException("Invalid username or password"));

        var handler = new LoginCommandHandler(_userQueryRepositoryMock.Object, _encryptionUtilityMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new LoginCommand { UserName = _username, Password = _password },
                    CancellationToken.None));
        
        _userQueryRepositoryMock.Verify(x => x.GetByUserName(_username, CancellationToken.None), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetSHA256(_password, _salt), Times.Never);
        _encryptionUtilityMock.Verify(x => x.GetNewToken(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task username_and_password_are_valid()
    {
        var request = _request();
        var user = new User(_username, _email, _hashedPassword, _salt);
        
        _userQueryRepositoryMock.Setup(x => x.GetByUserName(_username, CancellationToken.None))
            .ReturnsAsync(user);
        
        _encryptionUtilityMock.Setup(x => x.GetSHA256(_password, _salt))  
            .Returns(_hashedPassword);
        
        _encryptionUtilityMock.Setup(x => x.GetNewToken(user.Id)) 
            .Returns(_token);
        
        var handler = new LoginCommandHandler(_userQueryRepositoryMock.Object, _encryptionUtilityMock.Object);
        
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(result.UserName, _username);
        Assert.Equal(result.Token, _token);
        
        _userQueryRepositoryMock.Verify(x => x.GetByUserName(_username, CancellationToken.None), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetNewToken(user.Id), Times.Once);  
        _encryptionUtilityMock.Verify(x => x.GetSHA256(_password, _salt), Times.Once);
    }
    
    [Fact]
    public async Task user_and_password_are_invalid()
    {
        var user = new User(_username, _email, "_hashedPassword", _salt);
        
        _userQueryRepositoryMock.Setup(x => x.GetByUserName(_username, CancellationToken.None))
            .ReturnsAsync(user);
        
        _encryptionUtilityMock.Setup(x => x.GetSHA256(_password, _salt))  
            .Returns("wrong_hashed_password");
        
        var handler = new LoginCommandHandler(_userQueryRepositoryMock.Object, _encryptionUtilityMock.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new LoginCommand { UserName = _username, Password = _password },
                CancellationToken.None));
        
        _userQueryRepositoryMock.Verify(x => x.GetByUserName(_username, CancellationToken.None), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetSHA256(_password, _salt), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetNewToken(user.Id), Times.Never);  
    }
}