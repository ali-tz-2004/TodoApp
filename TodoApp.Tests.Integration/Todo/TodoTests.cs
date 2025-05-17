using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Todo.Commands.ChangeStatusTodoCommand;
using TodoApp.Application.Todo.Commands.CreateTodoCommand;
using TodoApp.Application.Todo.Commands.DeleteTodoCommand;
using TodoApp.Application.Todo.Commands.UpdateTodoCommand;
using TodoApp.Application.Todo.Queries.GetAllTodoQuery;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.ResponseHanlder;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Tests.Integration.Todo;

public class TodoTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
{
    public TodoTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task create_todo_when_successfully()
    {
        await AuthenticateAsync();
        var category = SeedCategory();

        var createCommand = new CreateTodoCommand
        {
            Title = "test",
            Description = "test",
            DueDate = new DateOnly(2025, 05, 17),
            CategoryId = category.Id,
            UserId = UserId,
        };

        var response = await Client.PostAsJsonAsync("/Todo/CreateTodo", createCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var todo = await CommandDbContext.TodoItems.AsNoTracking().SingleAsync();
        todo.Title.Should().Be(createCommand.Title);
        todo.Description.Should().Be(createCommand.Description);
        todo.DueDate.Should().Be(createCommand.DueDate);
        todo.CategoryId.Should().Be(createCommand.CategoryId);
        todo.UserId.Should().Be(UserId);
    }

    [Fact]
    public async Task create_todo_when_without_authorization()
    {
        var category = SeedCategory();

        var createCommand = new CreateTodoCommand
        {
            Title = "test",
            Description = "test",
            DueDate = new DateOnly(2025, 05, 17),
            CategoryId = category.Id,
            UserId = UserId,
        };

        var response = await Client.PostAsJsonAsync("/Todo/CreateTodo", createCommand);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var todos = await CommandDbContext.TodoItems.AsNoTracking().ToListAsync();
        todos.Should().BeEmpty();
    }

    [Fact]
    public async Task update_todo_when_successfully()
    {
        await AuthenticateAsync();
        var todo = SeedTodo();

        var updateTodoCommand = new UpdateTodoCommand
        {
            Id = todo.Id,
            Title = "test1",
            Description = "test1",
            DueDate = new DateOnly(2025, 05, 18),
            CategoryId = todo.CategoryId,
            UserId = UserId,
        };

        var response = await Client.PutAsJsonAsync("/Todo/UpdateTodo", updateTodoCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todoItem = await CommandDbContext.TodoItems.AsNoTracking().FirstAsync(x => x.Id == todo.Id);
        
        todoItem.Should().NotBeNull();
        todoItem.Title.Should().Be(updateTodoCommand.Title);
        todoItem.Description.Should().Be(updateTodoCommand.Description);
        todoItem.DueDate.Should().Be(updateTodoCommand.DueDate);
    }

    [Fact]
    public async Task update_todo_when_without_authorization()
    {
        var todo = SeedTodo();

        var updateTodoCommand = new UpdateTodoCommand
        {
            Id = todo.Id,
            Title = "test1",
            Description = "test1",
            DueDate = new DateOnly(2025, 05, 17),
            CategoryId = todo.CategoryId,
            UserId = UserId,
        };

        var response = await Client.PutAsJsonAsync("/Todo/UpdateTodo", updateTodoCommand);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var todoItem = await CommandDbContext.TodoItems.AsNoTracking().FirstAsync(x => x.Id == todo.Id);
        
        todoItem.Should().NotBeNull();
        todoItem.Title.Should().Be(todo.Title);
        todoItem.Description.Should().Be(todo.Description);
        todoItem.DueDate.Should().Be(todo.DueDate);
    }
    
    [Fact]
    public async Task get_all_todo_without_authorization()
    {
        var response = await Client.GetAsync("/Todo/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task get_all_todo_successfully()
    {
        await AuthenticateAsync();
        var todo = SeedTodo();

        var response = await Client.GetAsync($"/Todo/GetAll?Page=1&PageSize=10&UserId={UserId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PaginationResponse<GetAllTodoResponse>>>();
    
        content.Should().NotBeNull();
        content.Data!.Items.Should().HaveCount(1);
        content.Data.Items.First().Title.Should().Be(todo.Title);
        content.Data.Items.First().Description.Should().Be(todo.Description);
        content.Data.Items.First().DueDate.Should().Be(todo.DueDate);
    }
    
    [Fact]
    public async Task change_status_todo_when_without_authorization()
    {
        var todo = SeedTodo();

        var updateTodoCommand = new ChangeStatusTodoCommand()
        {
            Id = todo.Id,
            UserId = UserId,
        };

        var response = await Client.PutAsJsonAsync("/Todo/ChangeStatus", updateTodoCommand);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var todoItem = await CommandDbContext.TodoItems.AsNoTracking().FirstAsync(x => x.Id == todo.Id);
        
        todoItem.Should().NotBeNull();
        todoItem.IsCompleted.Should().Be(todo.IsCompleted);
    }
    
    [Fact]
    public async Task change_status_todo_when_valid()
    {
        await AuthenticateAsync();
        var todo = SeedTodo();

        var updateTodoCommand = new ChangeStatusTodoCommand()
        {
            Id = todo.Id,
            UserId = UserId,
        };

        var response = await Client.PutAsJsonAsync("/Todo/ChangeStatus", updateTodoCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var todoItem = await CommandDbContext.TodoItems.AsNoTracking().FirstAsync(x => x.Id == todo.Id);
        
        todoItem.Should().NotBeNull();
        todoItem.IsCompleted.Should().Be(!todo.IsCompleted);
    }
    
    [Fact]
    public async Task delete_todo_when_without_authorization()
    {
        var todo = SeedTodo();

        var updateTodoCommand = new DeleteTodoCommand()
        {
            Id = todo.Id,
            UserId = UserId,
        };

        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Todo/Delete", UriKind.Relative),
            Content = JsonContent.Create(updateTodoCommand)
        });
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var todoItem = await CommandDbContext.TodoItems.AsNoTracking().FirstAsync(x => x.Id == todo.Id);
        
        todoItem.Should().NotBeNull();
    }
    
    [Fact]
    public async Task delete_status_todo_when_valid()
    {
        await AuthenticateAsync();
        var todo = SeedTodo();

        var updateTodoCommand = new DeleteTodoCommand()
        {
            Id = todo.Id,
            UserId = UserId,
        };

        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Todo/Delete", UriKind.Relative),
            Content = JsonContent.Create(updateTodoCommand)
        });
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var todoItem = await CommandDbContext.TodoItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == todo.Id);
        
        todoItem.Should().BeNull();
    }
    
    

    public Category SeedCategory()
    {
        var category = Category.CreateCategory("test", UserId);

        CommandDbContext.Categories.Add(category);
        CommandDbContext.SaveChanges();

        return category;
    }

    public TodoItem SeedTodo()
    {
        var category = SeedCategory(); 
        
        var todo = TodoItem.CreateTodo("test", "test", new DateOnly(2025, 05, 17), category.Id, UserId);

        CommandDbContext.TodoItems.Add(todo);
        CommandDbContext.SaveChanges();

        return todo;
    }
}