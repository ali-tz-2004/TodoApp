using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TodoApp.Application.Todo.Commands.CreateCategoryCommand;

namespace TodoApp.Tests.Integration.Todo;

public class CategoryTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
{
    public CategoryTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task create_category_when_without_authorization()
    {
        var categoryCommand = new CreateCategoryCommand
        {
            Name = "Test",
            UserId = Guid.NewGuid()
        };

        var response = await Client.PostAsJsonAsync("/Category/CreateCategory", categoryCommand);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task create_category_successfully()
    {
        await AuthenticateAsync();

        var categoryCommand = new CreateCategoryCommand
        {
            Name = "Test",
            UserId = Guid.NewGuid()
        };

        var response = await Client.PostAsJsonAsync("/Category/CreateCategory", categoryCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}