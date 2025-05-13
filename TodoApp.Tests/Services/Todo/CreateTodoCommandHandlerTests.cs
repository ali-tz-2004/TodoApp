using Moq;
using TodoApp.Application.Todo.Commands.CreateTodoCommand;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class CreateTodoCommandHandlerTests
{
    private readonly Mock<ITodoCommandRepository> _todoCommandRepoMock = new();
    private readonly Mock<ITodoQueryRepository> _todoQueryRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private CreateTodoCommand CreateRequest(string title = "test title", string description = "test desc", DateOnly? dueDate = null,
        int categoryId = 1, Guid? userId = null)
        => new() { Title = title, Description = description, DueDate = DateOnly.FromDateTime(DateTime.Today), CategoryId = categoryId, UserId = Guid.NewGuid() };
    
    [Fact]
    public async Task create_todo_success()
    {
        var request = CreateRequest();

        _todoQueryRepoMock.Setup(x => x.ExistsCategoryAsync(request.CategoryId, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateTodoCommandHandler(
            _todoCommandRepoMock.Object,
            _todoQueryRepoMock.Object,
            _unitOfWorkMock.Object
        );

        await handler.Handle(request, CancellationToken.None);

        _todoCommandRepoMock.Verify(x => x.CreateTodo(It.IsAny<TodoItem>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task category_or_user_not_found()
    {
        var request = CreateRequest();

        _todoQueryRepoMock.Setup(x =>
            x.ExistsCategoryAsync(request.CategoryId, request.UserId, CancellationToken.None)).ReturnsAsync(false);
        
        var handler = new CreateTodoCommandHandler(
            _todoCommandRepoMock.Object,
            _todoQueryRepoMock.Object,
            _unitOfWorkMock.Object
        );
        
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        
        Assert.Contains("Category not found", ex.Message);
        
        _todoCommandRepoMock.Verify(x=>x.CreateTodo(It.IsAny<TodoItem>()), Times.Never);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task title_cannot_be_empty(string title)
    {
        var request = CreateRequest(title: title);

        _todoQueryRepoMock.Setup(x =>
            x.ExistsCategoryAsync(request.CategoryId, request.UserId, CancellationToken.None)).ReturnsAsync(true);
        
        var handler = new CreateTodoCommandHandler(
            _todoCommandRepoMock.Object,
            _todoQueryRepoMock.Object,
            _unitOfWorkMock.Object
        );
        
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        
        Assert.Contains("title cannot be empty.", ex.Message);
        
        _todoCommandRepoMock.Verify(x=>x.CreateTodo(It.IsAny<TodoItem>()), Times.Never);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task if_description_empty_string_must_be_converted_to_null(string description)
    {
        var request = CreateRequest(description: description);

        _todoQueryRepoMock.Setup(x =>
            x.ExistsCategoryAsync(request.CategoryId, request.UserId, CancellationToken.None)).ReturnsAsync(true);
        
        var handler = new CreateTodoCommandHandler(
            _todoCommandRepoMock.Object,
            _todoQueryRepoMock.Object,
            _unitOfWorkMock.Object
        );
        
        await handler.Handle(request, CancellationToken.None);
        
        _todoCommandRepoMock.Verify(x =>
            x.CreateTodo(It.Is<TodoItem>(t => t.Description == null)), Times.Once);
        
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task due_date_must_be_mapped_correctly()
    {
        var dueDate = new DateOnly(2025, 5, 10);
        var request = CreateRequest(dueDate: dueDate);

        _todoQueryRepoMock.Setup(x =>
            x.ExistsCategoryAsync(request.CategoryId, request.UserId, CancellationToken.None)).ReturnsAsync(true);

        var handler = new CreateTodoCommandHandler(
            _todoCommandRepoMock.Object,
            _todoQueryRepoMock.Object,
            _unitOfWorkMock.Object
        );

        await handler.Handle(request, CancellationToken.None);

        _todoCommandRepoMock.Verify(x =>
            x.CreateTodo(It.Is<TodoItem>(t => t.DueDate == dueDate)), Times.Once);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}