using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Infrastructure;

namespace TodoApp.Tests.Integration.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;


    public RegisterTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task CreateRegisterUser(string username = "userTest", string password = "passwordTest", string email = "test@gmail.com")
    {
        var command = new RegisterCommand()
        {
            UserName = username,
            Password = password,
            Email = email,
        };

        var response = await _client.PostAsJsonAsync("/Auth/Register", command);

        response.IsSuccessStatusCode.Should().BeTrue();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TodoAppCommandDbContext>();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == "test@gmail.com");

            user.Should().NotBeNull();
        }
    }
    
    private async Task ResetDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoAppCommandDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task register_with_valid_data_should_return_ok()
    {
        await ResetDatabaseAsync();
        await CreateRegisterUser();
    }

    [Fact]
    public async Task register_with_duplicate_username_should_return_Conflict()
    {
        await ResetDatabaseAsync();
        await CreateRegisterUser();

        var secondCommand = new RegisterCommand()
        {
            UserName = "userTest", 
            Password = "password",
            Email = "userTest@gmail.com",
        };

        var secondResponse = await _client.PostAsJsonAsync("/Auth/Register", secondCommand);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict); 
    }
    
    [Fact]
    public async Task register_with_duplicate_email_should_return_Conflict()
    {
        await ResetDatabaseAsync();
        await CreateRegisterUser();

        var secondCommand = new RegisterCommand()
        {
            UserName = "userTest2", 
            Password = "password",
            Email = "test@gmail.com",
        };

        var secondResponse = await _client.PostAsJsonAsync("/Auth/Register", secondCommand);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict); 
    }

}