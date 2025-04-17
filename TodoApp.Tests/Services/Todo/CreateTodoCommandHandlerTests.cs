using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
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

    private CreateTodoRequest CreateRequest() => new()
    {
        Title = "Test title",
        Description = "Test desc",
        DueDate = DateOnly.FromDateTime(DateTime.Today),
        CategoryId = 1,
        UserId = Guid.NewGuid()
    };
    
    [Fact]
    public async Task Handle_Should_Create_Todo_When_Valid()
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
    public async Task Handle_Should_Throw_NotFoundException_When_Category_Not_Exist()
    {
        var request = CreateRequest();

        _todoQueryRepoMock.Setup(x => x.ExistsCategoryAsync(request.CategoryId, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateTodoCommandHandler(
            _todoCommandRepoMock.Object,
            _todoQueryRepoMock.Object,
            _unitOfWorkMock.Object
        );

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(request, CancellationToken.None));
    }
}