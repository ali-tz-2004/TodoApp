using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TodoApp.Application.Todo.Commands.CreateCategoryCommand;
using TodoApp.Application.Todo.Commands.UpdateCategoryCommand;
using TodoApp.Core.Entities.Todo;

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
            UserId = UserId
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
            UserId = UserId
        };

        var response = await Client.PostAsJsonAsync("/Category/CreateCategory", categoryCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task update_category_when_without_authorization()
    {
        var categoryCommand = new UpdateCategoryCommand
        {
            Id = 1,
            Name = "Test",
            UserId = UserId
        };

        var response = await Client.PutAsJsonAsync("/Category/UpdateCategory", categoryCommand);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task update_category_successfully()
    {
        await AuthenticateAsync();
        var category = SeedCategory();
        
        var categoryCommand = new UpdateCategoryCommand
        {
            Id = category.Id,
            Name = "Test1",
            UserId = UserId
        };

        var response = await Client.PutAsJsonAsync("/Category/UpdateCategory", categoryCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public Category SeedCategory()
    {
        var category = Category.CreateCategory("Test", UserId);
        
        CommandDbContext.Categories.Add(category);
        CommandDbContext.SaveChanges();

        return category;
    }
}