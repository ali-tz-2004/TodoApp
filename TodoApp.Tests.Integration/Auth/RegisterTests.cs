using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Core.Entities.Auth;
using TodoApp.Infrastructure;

namespace TodoApp.Tests.Integration.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory>, IAsyncDisposable
{
    private readonly HttpClient _client;
    private readonly TodoAppCommandDbContext _commandDbContext;

    public RegisterTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        var services = factory.Services.CreateScope().ServiceProvider;
        _commandDbContext = services.GetRequiredService<TodoAppCommandDbContext>();
    }

    [Fact]
    public async Task register_with_valid_data_should_return_ok()
    {
        var command = new RegisterCommand()
        {
            UserName = "admin",
            Password = "password",
            Email = "admin@admin.com",
        };

        var response = await _client.PostAsJsonAsync("/Auth/Register", command);

        response.IsSuccessStatusCode.Should().BeTrue();

        var user = await _commandDbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Email);

        user.Should().NotBeNull();
    }

    [Fact]
    public async Task register_with_duplicate_username_should_return_Conflict()
    {
        var user = SeedUser();
        
        var command = new RegisterCommand()
        {
            UserName = user.UserName,
            Password = "password",
            Email = "userTest@gmail.com",
        };

        var response = await _client.PostAsJsonAsync("/Auth/Register", command);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task register_with_duplicate_email_should_return_Conflict()
    {
        var user = SeedUser();
        
        var command = new RegisterCommand()
        {
            UserName = "userTest2",
            Password = "password",
            Email = user.Email,
        };

        var response = await _client.PostAsJsonAsync("/Auth/Register", command);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private User SeedUser()
    {
        var user = User.CreateUser(email: "test@gmail.com", password: "password", userName: "test",
            passwordSalt: "salt");
        
        _commandDbContext.Users.Add(user);
        _commandDbContext.SaveChanges();
        
        return user;
    }

    public async ValueTask DisposeAsync()
    {
        await _commandDbContext.Database.EnsureDeletedAsync();
        await _commandDbContext.Database.EnsureCreatedAsync();
    }
}