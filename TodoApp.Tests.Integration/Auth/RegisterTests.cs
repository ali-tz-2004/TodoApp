using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Core.Entities.Auth;

namespace TodoApp.Tests.Integration.Auth;

public class RegisterTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
{
    public RegisterTests(CustomWebApplicationFactory factory) : base(factory)
    {
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

        var response = await Client.PostAsJsonAsync("/Auth/Register", command);

        response.IsSuccessStatusCode.Should().BeTrue();

        var user = await CommandDbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Email);

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

        var response = await Client.PostAsJsonAsync("/Auth/Register", command);
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

        var response = await Client.PostAsJsonAsync("/Auth/Register", command);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private User SeedUser()
    {
        var user = User.CreateUser(email: "test@gmail.com", password: "password", userName: "test",
            passwordSalt: "salt");
        
        CommandDbContext.Users.Add(user);
        CommandDbContext.SaveChanges();
        
        return user;
    }
}