using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Auth.Commands.LoginCommand;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Common.ResponseHanlder;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;

namespace TodoApp.Tests.Integration.Auth;

public class LoginTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
{
    private readonly IEncryptionUtility _encryptionUtility;

    public LoginTests(CustomWebApplicationFactory factory) : base(factory)
    {
        var services = factory.Services.CreateScope().ServiceProvider;
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
        
        var response = await Client.PostAsJsonAsync("/Auth/Login", command);
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

        var response = await Client.PostAsJsonAsync("/Auth/Login", loginCommand);
    
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
        
        CommandDbContext.Users.Add(user);
        CommandDbContext.SaveChanges();
        
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

        var response = await Client.PostAsJsonAsync("/Auth/Register", registerCommand);
        response.EnsureSuccessStatusCode();
    }
}