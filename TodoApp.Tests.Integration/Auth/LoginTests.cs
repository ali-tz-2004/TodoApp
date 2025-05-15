using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Auth.Commands.LoginCommand;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Common.ResponseHanlder;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Infrastructure;

namespace TodoApp.Tests.Integration.Auth;

public class LoginTests : IClassFixture<CustomWebApplicationFactory>,IAsyncDisposable
{
    private readonly HttpClient _client;
    private readonly TodoAppCommandDbContext _commandDbContext;
    private readonly IEncryptionUtility _encryptionUtility;


    public LoginTests(CustomWebApplicationFactory factory)
    {
        var services = factory.Services.CreateScope().ServiceProvider;
        _commandDbContext = services.GetRequiredService<TodoAppCommandDbContext>();
        _client = factory.CreateClient();
        _encryptionUtility = services.GetRequiredService<IEncryptionUtility>();
    }

    [Fact]
    public async Task when_password_wrong_then_login_should_fail()
    {
        var user = SeedUser();

        var command = new LoginCommand()
        {
            UserName = user.UserName,
            Password = "password"
        };
        var hashPassword = _encryptionUtility.GetSHA256(command.Password, user.PasswordSalt);
        
        Assert.NotEqual(hashPassword, user.Password);
        
        var response = await _client.PostAsJsonAsync("/Auth/Login", command);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task when_login_successful()
    {
        await CreateUserThroughApi();

        var loginCommand = new LoginCommand
        {
            UserName = "test",
            Password = "password" 
        };

        var response = await _client.PostAsJsonAsync("/Auth/Login", loginCommand);
    
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        result.Should().NotBeNull();
        result.Data.UserName.Should().Be("test");
        result.Data.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    private User SeedUser()
    {
        var user = User.CreateUser(email: "test@gmail.com", password: "password", userName: "test",
            passwordSalt: "salt");
        
        _commandDbContext.Users.Add(user);
        _commandDbContext.SaveChanges();
        
        return user;
    }
    
    private async Task CreateUserThroughApi(string username = "test", string password = "password", string email = "test@gmail.com")
    {
        var registerCommand = new RegisterCommand
        {
            UserName = username,
            Password = password,
            Email = email
        };

        var response = await _client.PostAsJsonAsync("/Auth/Register", registerCommand);
        response.EnsureSuccessStatusCode();
    }

    public async ValueTask DisposeAsync()
    {
        await _commandDbContext.Database.EnsureDeletedAsync();
        await _commandDbContext.Database.EnsureCreatedAsync();
    }
}