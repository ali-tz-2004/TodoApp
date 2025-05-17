using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Auth.Commands.LoginCommand;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Common.ResponseHanlder;
using TodoApp.Core.Entities.Auth;
using TodoApp.Infrastructure;
using TodoApp.Tests.Integration;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly TodoAppCommandDbContext CommandDbContext;
    private readonly IServiceScope _scope;
    private readonly CustomWebApplicationFactory _factory;
    protected Guid UserId;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        Client = factory.CreateClient();

        _scope = factory.Services.CreateScope();
        CommandDbContext = _scope.ServiceProvider.GetRequiredService<TodoAppCommandDbContext>();
    }

    protected async Task AuthenticateAsync()
    {
        var register = new RegisterCommand
        {
            UserName = "testuser",
            Password = "password",
            Email = "test@email.com"
        };

        await Client.PostAsJsonAsync("/Auth/Register", register);

        var login = new LoginCommand
        {
            UserName = "testuser",
            Password = "password"
        };

        var loginResponse = await Client.PostAsJsonAsync("/Auth/Login", login);
        var apiResponse = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        var token = apiResponse!.Data.Token;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var user = await CommandDbContext.Users.FirstAsync(u => u.UserName == "testuser");
        UserId = user.Id;
    }

    public async Task InitializeAsync()
    {
        await CommandDbContext.Database.EnsureDeletedAsync();
        await CommandDbContext.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }
}