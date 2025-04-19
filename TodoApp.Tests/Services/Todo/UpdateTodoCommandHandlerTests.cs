using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class UpdateTodoCommandHandlerTests
{
    private readonly Mock<ITodoCommandRepository> _todoCommandRepositoryMock = new();
    private readonly Mock<ITodoQueryRepository> _todoQueryRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private UpdateTodoRequest UpdateRequest() => new()
    {
        Id = 1,
        Title = "Test title",
        Description = "Test desc",
        DueDate = DateOnly.FromDateTime(DateTime.Today),
        CategoryId = 3,
        UserId = Guid.NewGuid()
    };

    [Fact]
    public async Task Handle_Should_Update_Todo_When_Valid()
    {
        var request = UpdateRequest();
        var fakeTodoItem = new TodoItem("Old Title", "Old Desc", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), request.CategoryId, request.UserId);

        _todoQueryRepository.Setup(x =>
                x.ExistsCategoryAsync(request.CategoryId, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    
        _todoQueryRepository.Setup(x => x.GetById(request.Id, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeTodoItem);

        var handler = new UpdateTodoCommandHandler(
            _todoCommandRepositoryMock.Object,
            _todoQueryRepository.Object,
            _unitOfWorkMock.Object);
    
        await handler.Handle(request, CancellationToken.None);
    
        _todoCommandRepositoryMock.Verify(x => x.UpdateTodo(It.IsAny<TodoItem>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_Category_Not_Exists()
    {
        // Arrange
        var request = UpdateRequest();

        _todoQueryRepository.Setup(x =>
                x.ExistsCategoryAsync(request.CategoryId, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new UpdateTodoCommandHandler(
            _todoCommandRepositoryMock.Object,
            _todoQueryRepository.Object,
            _unitOfWorkMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
    }
}