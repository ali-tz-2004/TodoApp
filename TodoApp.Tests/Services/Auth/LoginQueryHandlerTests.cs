using Moq;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Application.Services.Auth.Query;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Tests.Services.Auth;

public class LoginQueryHandlerTests
{
    private Mock<IUserQueryRepository> _userQueryRepositoryMock = new();
    private Mock<IEncryptionUtility> _encryptionUtilityMock = new();

    private string _username = "user";
    private string _email = "user";
    private string _password = "123";
    private string _salt = "random_salt";
    private string _hashedPassword = "hashed_123";
    private string _token = "fake_token";
    
    private LoginRequest _request() => new()
    {
        UserName = _username,
        Password = _password
    };

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowNotFoundException()
    {
        _userQueryRepositoryMock.Setup(x => x.Login(_username, _password, CancellationToken.None))
            .Throws(new NotFoundException("Invalid username or password"));

        var handler = new LoginQueryHandler(_userQueryRepositoryMock.Object, _encryptionUtilityMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new LoginRequest { UserName = _username, Password = _password },
                    CancellationToken.None));
        
        _userQueryRepositoryMock.Verify(x => x.Login(_username, _password, CancellationToken.None), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetSHA256(_password, _salt), Times.Never);
        _encryptionUtilityMock.Verify(x => x.GetNewToken(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UserFoundAndPasswordMatches_ShouldReturnLoginResponse()
    {
        var request = _request();
        var user = new User(_username, _email, _hashedPassword, _salt);
        
        _userQueryRepositoryMock.Setup(x => x.Login(_username, _password, CancellationToken.None))
            .ReturnsAsync(user);
        
        _encryptionUtilityMock.Setup(x => x.GetSHA256(_password, _salt))  
            .Returns(_hashedPassword);
        
        _encryptionUtilityMock.Setup(x => x.GetNewToken(user.Id)) 
            .Returns(_token);
        
        var handler = new LoginQueryHandler(_userQueryRepositoryMock.Object, _encryptionUtilityMock.Object);
        
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(result.UserName, _username);
        Assert.Equal(result.Token, _token);
        
        _userQueryRepositoryMock.Verify(x => x.Login(_username, _password, CancellationToken.None), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetNewToken(user.Id), Times.Once);  
        _encryptionUtilityMock.Verify(x => x.GetSHA256(_password, _salt), Times.Once);
    }
    
    [Fact]
    public async Task Handle_UserFoundAndPasswordDontMatches_ShouldThrowNotFoundException()
    {
        var user = new User(_username, _email, "_hashedPassword", _salt);
        
        _userQueryRepositoryMock.Setup(x => x.Login(_username, _password, CancellationToken.None))
            .ReturnsAsync(user);
        
        _encryptionUtilityMock.Setup(x => x.GetSHA256(_password, _salt))  
            .Returns("wrong_hashed_password");
        
        var handler = new LoginQueryHandler(_userQueryRepositoryMock.Object, _encryptionUtilityMock.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new LoginRequest { UserName = _username, Password = _password },
                CancellationToken.None));
        
        _userQueryRepositoryMock.Verify(x => x.Login(_username, _password, CancellationToken.None), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetSHA256(_password, _salt), Times.Once);
        _encryptionUtilityMock.Verify(x => x.GetNewToken(user.Id), Times.Never);  
    }
}